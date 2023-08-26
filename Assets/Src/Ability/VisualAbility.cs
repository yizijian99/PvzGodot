using Godot;
using GodotUtilities;

[Scene]
public partial class VisualAbility : Node
{
    [Signal]
    public delegate void TargetInViewEventHandler(GodotObject target);

    [Signal]
    public delegate void TargetOutOfViewEventHandler();

    [Export]
    private RayCast2D rayCast2D;

    private GodotObject target;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Check();
    }

    private void Check()
    {
        GodotObject newValue = rayCast2D.GetCollider();
        if (target != newValue)
        {
            target = newValue;
            if (newValue != null)
            {
                EmitSignal(SignalName.TargetInView, target);
            }
            else
            {
                EmitSignal(SignalName.TargetOutOfView);
            }
        }
    }
}
