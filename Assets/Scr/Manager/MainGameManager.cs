using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using GodotUtilities;
using Pvz.Assets.Scr.Autoload;
using Pvz.Assets.Scr.Card;
using Pvz.Assets.Scr.Effect;
using Pvz.Assets.Scr.HUD;
using Pvz.Assets.Scr.Support;

namespace Pvz.Assets.Scr.Manager;

[Scene]
public sealed partial class MainGameManager : Node
{
    [Export]
    private Node sceneRoot;

    [Export]
    private Camera2D camera2D;

    [Export]
    private Marker2D startCameraMarker;

    [Export]
    private Marker2D combatCameraMarker;

    [Export]
    private Marker2D preparationCameraMarker;

    [Export]
    private Marker2D cardBarMarker;

    [Export]
    private Marker2D cardPoolMarker;

    [Export]
    private CanvasLayer hud;

    [Export]
    private CardBar cardBar;

    [Export]
    private CardPool cardPool;

    [Export]
    private Label sunCounter;

    [Export]
    private BaseButton letsRockButton;

    [Export]
    private TextureRect reminder;

    [Export]
    private Marker2D sunCollectorMarker;

    [Export]
    private Sprite2D plantCursor;

    [Export]
    private GridContainer gridContainer;

    [Export]
    private Node2D ground;

    [Export]
    private Array<Texture> combatStageRemindTextures;

    [Export(PropertyHint.File, "*.tscn")]
    private string gridScenePath;

    private int sunCount = 50;

    private BaseCard selectedCard;

    public BaseCard SelectedCard
    {
        get => selectedCard;
        private set
        {
            if (selectedCard == null)
            {
                selectedCard = value;
                plantCursor.Texture = value?.Texture;
            }
            else
            {
                selectedCard = null;
                plantCursor.Texture = null;
            }
        }
    }

    [Export]
    public int SunCount
    {
        get => sunCount;
        set
        {
            sunCount = value;
            sunCounter.Text = sunCount.ToString();
            SignalBus.Instance.EmitSignal(SignalBus.SignalName.SunCountChanged, sunCount);
        }
    }

    private readonly Queue<Action> nextFrameActionQueue = new();

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        sceneRoot.Ready += EnterPreparationStage;
        SignalBus.Instance.CardToCombat += OnCardToCombat;
        SignalBus.Instance.CardToCandidate += OnCardToCandidate;
        letsRockButton.Pressed += LetsRock;
        SignalBus.Instance.SunPicked += OnSunPicked;
        SignalBus.Instance.CardReadyToPlant += OnCardReadyToPlant;

        camera2D.GlobalPosition = startCameraMarker.GlobalPosition;
        sunCounter.Text = SunCount.ToString();
        InitializeGridContainer();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        while (nextFrameActionQueue.Count > 0)
        {
            nextFrameActionQueue.Dequeue()?.Invoke();
        }

        UpdateLetsRockButton();
        plantCursor.GlobalPosition = GetViewport().GetMousePosition();
    }

    private async void EnterPreparationStage()
    {
        SceneTransitionMask.Instance.Enable();

        await CameraTransition.Instance.TransitionCamera2D(camera2D, preparationCameraMarker, 2.5);
        await CardControlAnimation();

        SceneTransitionMask.Instance.Disable();
    }

    private async Task EnterCombatStage()
    {
        SceneTransitionMask.Instance.Enable();

        await CardControlAnimation();
        await CameraTransition.Instance.TransitionCamera2D(camera2D, combatCameraMarker, 1.5);

        SceneTransitionMask.Instance.Disable();
    }

    private async Task CardControlAnimation()
    {
        double duration = 0.15;
        Vector2 cardBarMarkerGlobalPosition = cardBarMarker.GlobalPosition;
        Vector2 cardPoolMarkerGlobalPosition = cardPoolMarker.GlobalPosition;
        cardPoolMarker.GlobalPosition = cardPool.GlobalPosition;
        Tween tween = CreateTween();
        tween.SetParallel();
        tween.TweenProperty(cardBar, Node2D.PropertyName.GlobalPosition.ToString(), cardBarMarkerGlobalPosition,
            duration);
        tween.TweenProperty(cardPool, Node2D.PropertyName.GlobalPosition.ToString(), cardPoolMarkerGlobalPosition,
            duration);
        await ToSignal(tween, Tween.SignalName.Finished);
        foreach (BaseCard card in cardPool.GetAll())
        {
            card.State = BaseCard.CardState.Candidate;
        }
    }

    private void OnCardToCombat(BaseCard card)
    {
        if (cardBar.Length >= cardBar.MaxCapacity)
        {
            return;
        }

        BaseCard combatCard = ResourceLoader.Load<PackedScene>(card.SceneFilePath)?.InstantiateOrNull<BaseCard>();
        if (combatCard == null)
        {
            return;
        }

        if (cardBar.Add(combatCard))
        {
            card.Disabled = true;
            combatCard.State = BaseCard.CardState.ReadyToCombat;
            Color originalModulate = combatCard.Modulate;
            combatCard.Modulate = new Color(originalModulate, 0);
            CardTransitionAnimation(card, combatCard, 0.2,
                () => combatCard.Modulate = new Color(originalModulate));
        }
    }

    private void OnCardToCandidate(BaseCard card)
    {
        BaseCard first = cardPool.GetAll()
            .First(v => v.SceneFilePath == card.SceneFilePath);
        card.Visible = false;
        card.State = BaseCard.CardState.Default;
        CardTransitionAnimation(card, first, 0.2, () =>
        {
            first.Disabled = false;
            cardBar.Remove(card);
        });
    }

    public void CardTransitionAnimation(BaseCard from, BaseCard to, double duration, Action action = null)
    {
        Texture2D texture = from.TextureNormal;
        Vector2 fromGlobalPosition = from.GetGlobalRect().GetCenter();
        nextFrameActionQueue.Enqueue(() =>
        {
            Sprite2D sprite2D = new();
            sprite2D.Texture = texture;
            sprite2D.GlobalPosition = fromGlobalPosition;
            hud.AddChild(sprite2D);
            Tween tween = CreateTween();
            tween.TweenProperty(sprite2D, Node2D.PropertyName.GlobalPosition.ToString(), to.GetGlobalRect().GetCenter(),
                duration);
            tween.TweenCallback(Callable.From(() =>
            {
                sprite2D.Visible = false;
                sprite2D.QueueFree();
                action?.Invoke();
            }));
        });
    }

    private void OnCardReadyToPlant(BaseCard card)
    {
        if (card.Cost <= SunCount)
        {
            SelectedCard = card;
        }
    }

    public void UpdateLetsRockButton()
    {
        letsRockButton.Disabled = cardBar.Length < cardBar.MaxCapacity;
        letsRockButton.Modulate = new Color(letsRockButton.Modulate, letsRockButton.Disabled ? 0.7f : 1);
    }

    public async void LetsRock()
    {
        await EnterCombatStage();

        SceneTransitionMask.Instance.Enable();

        double interval = 0.5;
        Tween tween = CreateTween();
        foreach (Texture texture in combatStageRemindTextures)
        {
            tween.TweenProperty(reminder, TextureRect.PropertyName.Texture.ToString(), texture, 0);
            tween.TweenInterval(interval);
        }

        await ToSignal(tween, Tween.SignalName.Finished);
        reminder.Texture = null;

        SceneTransitionMask.Instance.Disable();

        SignalBus.Instance.EmitSignal(SignalBus.SignalName.MainGameStarted);
        SignalBus.Instance.EmitSignal(SignalBus.SignalName.SunCountChanged, sunCount);
    }

    private void OnSunPicked(Sun sun)
    {
        sun.GetParent().RemoveChild(sun);
        hud.AddChild(sun);
        Tween tween = CreateTween();
        tween.SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
        tween.TweenProperty(sun, Node2D.PropertyName.GlobalPosition.ToString(), sunCollectorMarker.GlobalPosition, 0.5)
            .From(sun.GlobalPosition - (GetViewport().GetCamera2D().GlobalPosition -
                                        GetViewport().GetCamera2D().GetViewportRect().GetCenter()));
        tween.TweenCallback(Callable.From(() =>
        {
            SunCount += sun.SunValue;
            sun.Visible = false;
            sun.QueueFreeDeferred();
        }));
    }

    private void InitializeGridContainer()
    {
        int count = 9 * 5;
        Vector2 gridSize = new(77, 90);
        for (int i = 0; i < count; i++)
        {
            Grid grid = ResourceLoader.Load<PackedScene>(gridScenePath)?.InstantiateOrNull<Grid>();
            if (grid == null)
            {
                GD.PushWarning($"load grid scene error. path: {gridScenePath}");
                continue;
            }
            grid.CustomMinimumSize = gridSize;
            grid.GuiInput += @event => OnGridGuiInput(grid, @event);
            gridContainer.AddChildDeferred(grid);
        }
    }

    private void OnGridGuiInput(Grid grid, InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton
            && mouseButton.Pressed
            && mouseButton.ButtonIndex == MouseButton.Left)
        {
            if (SelectedCard == null || SelectedCard.Cost > SunCount) return;
            string entityScenePath = selectedCard.EntityScenePath;
            bool putted = grid.Place(entityScenePath, ground);
            if (putted)
            {
                SunCount -= SelectedCard.Cost;
                SelectedCard.CoolDown();
                SelectedCard = null;
            }
        }
    }
}