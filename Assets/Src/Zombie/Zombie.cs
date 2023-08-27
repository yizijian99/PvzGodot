using Godot;
using GodotUtilities;

[Scene]
public partial class Zombie : CharacterBody2D, HitHandler
{
    [Export]
    private Vector2 moveVelocity = Vector2.Zero;

    [Node("AnimatedSprite2D")]
    private AnimatedSprite2D animatedSprite2D;

    [Node("AnimationPlayer")]
    private AnimationPlayer animationPlayer;

    [Node("HurtBox")]
    private HurtBox hurtBox;

    [Node("VisibleOnScreenNotifier2D")]
    private VisibleOnScreenNotifier2D visibleOnScreenNotifier2D;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        visibleOnScreenNotifier2D.ScreenEntered += () => hurtBox.enable = true;
        visibleOnScreenNotifier2D.ScreenExited += () => hurtBox.enable = false;

        hurtBox.enable = visibleOnScreenNotifier2D.IsOnScreen();
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

    public HitRequest buildHitRequest()
    {
        return null;
    }

    public void DoHitRequest(HitRequest request, HitResponse response)
    {
        GD.Print($"The zombie has received {request.damage} damage");
    }

    public void DoHitResponse(HitResponse response)
    {

    }
}
