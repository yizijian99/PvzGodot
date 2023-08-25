using Godot;
using GodotUtilities;

[Scene]
public partial class Plant : Node2D
{
    public override void _Ready()
    {
        base._Ready();
        WireNodes();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    protected void Dead()
    {
        QueueFree();
        SignalBus.Instance.EmitSignal(SignalBus.SignalName.Plant_Dead, this);
    }
}
