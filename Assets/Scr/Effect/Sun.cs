using Godot;
using GodotUtilities;
using Pvz.Assets.Scr.Autoload;

namespace Pvz.Assets.Scr.Effect;

[Scene]
public partial class Sun : RigidBody2D
{
    [Export]
    private float surviveTime = 10;

    [Export]
    public int SunValue { get; set; } = 50;

    [Node("Timer")]
    private Timer timer;

    [Node("AnimatedSprite2D")]
    private AnimatedSprite2D animatedSprite2D;

    public float FinalY { get; set; }

    private bool landed;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        timer.Timeout += QueueFree;
        InputEvent += OnInputEvent;

        RandomFinalY();
    }

    private void RandomFinalY()
    {
        FinalY = new RandomNumberGenerator().RandfRange(GlobalPosition.Y, 560);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!landed && GlobalPosition.Y >= FinalY)
        {
            LinearVelocity = Vector2.Zero;
            landed = true;
            timer.WaitTime = surviveTime;
            timer.Start();
        }
    }

    private void OnInputEvent(Node viewport, InputEvent @event, long shapeidx)
    {
        if (@event is InputEventMouseButton mouseButton
            && mouseButton.Pressed
            && mouseButton.ButtonIndex == MouseButton.Left)
        {
            Pick();
        }
    }

    public void Pick()
    {
        InputPickable = false;
        LinearVelocity = Vector2.Zero;
        SignalBus.Instance.EmitSignal(SignalBus.SignalName.SunPicked, this);
    }
}