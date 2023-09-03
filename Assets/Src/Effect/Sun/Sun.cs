using Godot;
using GodotUtilities;

[Scene]
public partial class Sun : RigidBody2D
{
    [Export]
    public int value { get; private set; } = 25;

    [Export]
    public float surviveTime { get; private set; }

    [Node("Control")]
    protected Control control;

    [Node("Timer")]
    protected Timer timer;

    private bool land = false;

    public float fallDistanceLimit;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        control.GuiInput += OnControlGuiInput;
        timer.Timeout += () => QueueFree();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!land && LinearVelocity.Y > 0 && Position.Y >= fallDistanceLimit)
        {
            land = true;
            GravityScale = 0;
            LinearVelocity = Vector2.Zero;
            if (surviveTime > 0)
            {
                timer.WaitTime = surviveTime;
                timer.Start();
            }
        }
    }

    protected void OnControlGuiInput(InputEvent @event)
    {
        if (InputUtils.MouseLeftButtonPressed(@event))
        {
            PickUp();
        }
    }

    protected void PickUp()
    {
        SignalBus.Instance.EmitSignal(SignalBus.SignalName.Sun_PickUp, value);
        QueueFree();
    }
}
