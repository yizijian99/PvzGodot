using Godot;
using GodotUtilities;
using Pvz.Assets.Scr.Autoload;

namespace Pvz.Assets.Scr.Effect;

[Tool]
[Scene]
public partial class Sun : RigidBody2D
{
    [Export]
    private float surviveTime = 10;

    [Export]
    public int SunValue { get; set; } = 50;

    [Export]
    public bool UpdateControl
    {
        get => false;
        set
        {
            Vector2? size = collisionShape2D?.Shape?.GetRect().Size;
            if (control != null)
            {
                if (size != null)
                {
                    control.Size = (Vector2)size;
                    control.Position = Vector2.Zero - new Vector2(control.Size.X / 2, control.Size.Y / 2);
                }
                else
                {
                    control.Size = Vector2.Zero;
                    control.Position = Vector2.Zero;
                }

                if (value)
                {
                    GD.Print("update control transform.");
                }
            }
        }
    }

    [Node("Timer")]
    private Timer timer;

    [Node("AnimatedSprite2D")]
    private AnimatedSprite2D animatedSprite2D;

    [Node("CollisionShape2D")]
    private CollisionShape2D collisionShape2D;

    [Node("Control")]
    private Control control;

    public float FinalY { get; set; }

    private bool landed;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        timer.Timeout += QueueFree;
        control.GuiInput += OnControlGuiInputEvent;

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

    private void OnControlGuiInputEvent(InputEvent @event)
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
        control.MouseFilter = Control.MouseFilterEnum.Ignore;
        LinearVelocity = Vector2.Zero;
        SignalBus.Instance.EmitSignal(SignalBus.SignalName.SunPicked, this);
    }
}