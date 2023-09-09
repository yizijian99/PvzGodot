using System;
using Godot;
using GodotUtilities;

[Scene]
public partial class MainGameManager : Node, HitHandler
{
    [Export]
    private int _totalSuns;
    public int totalSuns
    {
        get => _totalSuns;
        set
        {
            int oldValue = _totalSuns;
            int newValue = value;
            bool totalSunsChanged = oldValue != newValue;
            _totalSuns = newValue;
            if (totalSunsChanged)
                SignalBus.Instance.EmitSignal(SignalBus.SignalName.Game_TotalSunsChanged, oldValue, newValue);
        }
    }

    private Card _selectedCard;
    public Card selectedCard
    {
        get => _selectedCard;
        set
        {
            Card oldValue = _selectedCard;
            Card newValue = value;
            _selectedCard = newValue;
            if (oldValue != newValue)
            {
                SignalBus.Instance.EmitSignal(SignalBus.SignalName.Game_SelectedCardChanged, oldValue, newValue);
            }
        }
    }

    [Export]
    private Camera2D startStageCamera2D;

    [Export]
    private Camera2D preparationStageCamera2D;

    [Export]
    private Camera2D inGameStageCamera2D;

    [Export]
    private StageReminder stageReminder;

    [Export]
    private Chooser chooser;

    [Export]
    private Control cardSelector;

    [Export]
    private BaseButton letsRockButton;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        SignalBus.Instance.Sun_PickUp += amount => totalSuns += amount;
        SignalBus.Instance.Card_NodeReady += () => SignalBus.Instance.EmitSignal(SignalBus.SignalName.Game_TotalSunsChanged, _totalSuns, _totalSuns);
        SignalBus.Instance.Ground_GridBeClicked += OnGroundGridBeClicked;
        SignalBus.Instance.Card_BeClicked += OnCardBeClicked;
        SignalBus.Instance.TotalSunsLabel_NodeReady += () => SignalBus.Instance.EmitSignal(SignalBus.SignalName.Game_TotalSunsChanged, _totalSuns, _totalSuns);
        letsRockButton.Pressed += OnLetsRockButtonPressed;

        ScreenRect.Instance.Enable();
        startStageCamera2D.MakeCurrent();
        CameraTransition.Instance.TransitionCamera2D(startStageCamera2D, preparationStageCamera2D, 2.5f, async () =>
        {
            double duration = 0.2;
            Tween tween = CreateTween();
            tween.SetParallel();
            tween.TweenProperty(chooser, (string)Node2D.PropertyName.Position, new Vector2(chooser.Position.X, 0), duration);
            tween.TweenProperty(cardSelector, (string)Node2D.PropertyName.Position, new Vector2(cardSelector.Position.X, 90), duration);
            await ToSignal(tween, Tween.SignalName.Finished);
            ScreenRect.Instance.Disable();
        });
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        InputUtils.SetCustomMouseCursor(selectedCard?.preview);
    }

    private void OnGroundGridBeClicked(Grid grid)
    {
        if (grid == null) return;
        if (selectedCard == null) return;
        if (selectedCard.cost > totalSuns)
        {
            return;
        }
        bool planted = grid.Plant(selectedCard);
        if (!planted)
        {
            return;
        }
        totalSuns -= selectedCard.cost;
        selectedCard.CoolDown();
        selectedCard = null;
    }

    private void OnCardBeClicked(Card card)
    {
        if (selectedCard == card)
        {
            selectedCard = null;
            return;
        }
        selectedCard = card;
    }

    private void OnLetsRockButtonPressed()
    {
        LetsRock();
    }

    public HitRequest buildHitRequest()
    {
        return null;
    }

    public void DoHitRequest(HitRequest request, HitResponse response)
    {
        if (request != null)
        {
            // game over
            // GetTree().Quit();
        }
    }

    public void DoHitResponse(HitResponse response)
    {

    }

    private async void LetsRock()
    {
        ScreenRect.Instance.Enable();
        double duration = 0.2;
        Tween tween = CreateTween();
        tween.TweenProperty(cardSelector, (string)Node2D.PropertyName.Position, new Vector2(cardSelector.Position.X, 600), duration);
        await ToSignal(tween, Tween.SignalName.Finished);
        CameraTransition.Instance.TransitionCamera2D(preparationStageCamera2D, inGameStageCamera2D, 2f, () =>
        {
            stageReminder.PlayStartAnimation();
        });
    }
}
