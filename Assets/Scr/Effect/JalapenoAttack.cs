using Godot;
using GodotUtilities;
using Pvz.Assets.Scr.Zombies;

namespace Pvz.Assets.Scr.Effect;

[Scene]
public partial class JalapenoAttack : Node2D
{
    [Export]
    private AnimatedSprite2D left;

    [Export]
    private AnimatedSprite2D right;

    [Export]
    private Area2D area2D;

    private int state;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        left.AnimationFinished += () => state++;
        right.AnimationFinished += () => state++;
        left.FrameChanged += () =>
        {
            if (left.Animation == "default" && left.Frame == 2)
            {
                area2D.Monitoring = false;
                area2D.Monitorable = false;
            }
        };
        area2D.BodyEntered += body =>
        {
            if (body is Zombie)
            {
                body.CallDeferred(Node.MethodName.QueueFree);
            }
        };
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (state == 3)
        {
            return;
        }

        if (state == 2)
        {
            state++;
            CallDeferred(Node.MethodName.QueueFree);
        }
    }
}