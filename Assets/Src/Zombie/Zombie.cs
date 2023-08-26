using Godot;
using GodotUtilities;

[Scene]
public partial class Zombie : CharacterBody2D
{
    [Export]
    private Vector2 moveVelocity = Vector2.Zero;

    [Node("AnimatedSprite2D")]
    private AnimatedSprite2D animatedSprite2D;

    [Node("AnimationPlayer")]
    private AnimationPlayer animationPlayer;

    [Node("CollisionShape2D")]
    private CollisionShape2D collisionShape2D;

    [Node("VisibleOnScreenNotifier2D")]
    private VisibleOnScreenNotifier2D visibleOnScreenNotifier2D;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        visibleOnScreenNotifier2D.ScreenEntered += () => collisionShape2D.Disabled = false;
        visibleOnScreenNotifier2D.ScreenExited += () => collisionShape2D.Disabled = true;

        collisionShape2D.Disabled = !visibleOnScreenNotifier2D.IsOnScreen();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Velocity = moveVelocity * Vector2.Left * animationPlayer.SpeedScale;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        MoveAndSlide();
    }
}
