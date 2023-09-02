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

    [Node("HealthComponent")]
    private HealthComponent healthComponent;

    [Node("VisualAbility")]
    private VisualAbility visualAbility;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        visibleOnScreenNotifier2D.ScreenEntered += () => hurtBox.enable = true;
        visibleOnScreenNotifier2D.ScreenExited += () => hurtBox.enable = false;
        healthComponent.ZeroHealth += () => QueueFree();
        visualAbility.TargetInView += target =>
        {
            if (target is HurtBox hurtBox && hurtBox?.GetCollisionBoxOwner() is Plant)
            {
                animationPlayer.Play("Eat");
            }
        };
        visualAbility.TargetOutOfView += () =>
        {
            animationPlayer.Play("Move1");
        };

        hurtBox.enable = visibleOnScreenNotifier2D.IsOnScreen();
        animationPlayer.Play("Move1");
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
        HitRequest request = new HitRequest();
        request.damage = 1;
        return request;
    }

    public void DoHitRequest(HitRequest request, HitResponse response)
    {
        healthComponent.health -= request.damage;
    }

    public void DoHitResponse(HitResponse response)
    {

    }

    public void TakeABite()
    {
        GodotObject target = visualAbility.target;
        if (target == null)
            return;
        if (target is HurtBox hurtBox)
        {
            hurtBox?.GetCollisionBoxOwner()?.DoHitRequest(buildHitRequest(), new HitResponse());
        }
    }
}
