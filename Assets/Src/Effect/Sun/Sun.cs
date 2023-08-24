using Godot;
using GodotUtilities;

[Scene]
public partial class Sun : RigidBody2D
{
    [Export]
    protected int value = 25;

    [Node("Control")]
    protected Control control;

    public float fallDistanceLimit;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        control.GuiInput += OnControlGuiInput;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (LinearVelocity.Y > 0 && Position.Y >= fallDistanceLimit)
        {
            GravityScale = 0;
            LinearVelocity = Vector2.Zero;
        }
    }

    protected void OnControlGuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                PickUp();
            }
        }
    }

    protected void PickUp()
    {
        SignalBus.Instance.EmitSignal(SignalBus.SignalName.SunPicked, value);
        QueueFree();
    }
}
