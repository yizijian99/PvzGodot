using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotUtilities;
using Pvz.Assets.Scr.Autoload;
using Pvz.Assets.Scr.Card;
using Pvz.Assets.Scr.HUD;

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
    private CanvasLayer hud;

    [Export]
    private CardBar cardBar;

    [Export]
    private CardPool cardPool;

    private readonly Queue<Action> nextFrameActionQueue = new();

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        sceneRoot.Ready += EnterPreparationStage;
        SignalBus.Instance.CardToCombat += OnCardToCombat;
        SignalBus.Instance.CardToCandidate += OnCardToCandidate;

        camera2D.GlobalPosition = startCameraMarker.GlobalPosition;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        while (nextFrameActionQueue.Count > 0)
        {
            nextFrameActionQueue.Dequeue()?.Invoke();
        }
    }

    private void EnterPreparationStage()
    {
        CameraTransition.Instance.TransitionCamera2D(camera2D, preparationCameraMarker, 2.5);
    }

    private void OnCardToCombat(BaseCard card)
    {
        if (cardBar.Length() >= cardBar.MaxCapacity)
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
        BaseCard first = cardPool.GetAllCard()
            .First(v => v.SceneFilePath == card.SceneFilePath);
        card.Modulate = new Color(card.Modulate, 0);
        card.State = BaseCard.CardState.Default;
        CardTransitionAnimation(card, first, 0.2, () =>
        {
            first.Disabled = false;
            card.QueueFreeDeferred();
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
                sprite2D.QueueFreeDeferred();
                action?.Invoke();
            }));
        });
    }
}