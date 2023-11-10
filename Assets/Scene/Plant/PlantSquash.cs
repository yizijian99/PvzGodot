using Godot;
using GodotUtilities;
using Pvz.Assets.Scr.Ability;
using Pvz.Assets.Scr.Plant;

namespace Pvz.Assets.Scene.Plant;

[Scene]
public partial class PlantSquash : BasePlant
{
    private static readonly string AttackAnimation = "attack";

    [Node("TwoWayDetectAbility")]
    private BaseDetectAbility detectAbility;

    [Export]
    private AnimatedSprite2D animatedSprite2D;

    private bool isAttacking;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        detectAbility.Detected += zombie => { Attack(zombie); };

        animatedSprite2D.AnimationFinished += () =>
        {
            if (animatedSprite2D.Animation == AttackAnimation)
            {
                QueueFree();
            }
        };
    }

    public void Attack(Node2D target)
    {
        if (isAttacking)
        {
            return;
        }

        isAttacking = true;
        Tween tween = CreateTween();
        tween.TweenInterval(1);
        tween.TweenCallback(Callable.From(() =>
        {
            ZIndex = target.ZIndex + 1;
            animatedSprite2D.Stop();
            animatedSprite2D.Frame = 0;

            Vector2 globalPosition = GlobalPosition;
            Vector2 targetPosition = new(target.GlobalPosition.X, GlobalPosition.Y - 100);
            Tween t = CreateTween();
            t.TweenProperty(this, Node2D.PropertyName.GlobalPosition.ToString(), targetPosition, 0.3);
            t.TweenCallback(Callable.From(() =>
            {
                GlobalPosition = new Vector2(GlobalPosition.X, globalPosition.Y);
                animatedSprite2D.Play(AttackAnimation);
            }));
        }));
    }
}