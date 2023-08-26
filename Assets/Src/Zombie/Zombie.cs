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

    public override void _Ready()
    {
        base._Ready();
        WireNodes();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Velocity = moveVelocity * Vector2.Left * animationPlayer.SpeedScale;
        MoveAndSlide();
    }
}
