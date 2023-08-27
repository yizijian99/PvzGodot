using Godot;
using GodotUtilities;

[Scene]
public partial class Peashooter : Plant
{
    [Node("EmitBulletAbility")]
    private EmitBulletAbility emitBulletAbility;

    [Node("VisualAbility")]
    private VisualAbility visualAbility;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        emitBulletAbility.timer.Timeout += OnEmitBulletAbilityReady;
        visualAbility.TargetInView += target => emitBulletAbility.enable = true;
        visualAbility.TargetOutOfView += () => emitBulletAbility.enable = false;

        emitBulletAbility.SetTargetNode(GetTree().GetFirstNodeInGroup<Node2D>(GlobalExport.Instance.onGround));
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    private void OnEmitBulletAbilityReady()
    {
        emitBulletAbility.EmitBullet();
    }
}
