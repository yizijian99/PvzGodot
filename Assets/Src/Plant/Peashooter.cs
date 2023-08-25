using Godot;
using GodotUtilities;

[Scene]
public partial class Peashooter : Plant
{
    [Export]
    private PackedScene bulleScene;

    [Node("EmitBulletAbility")]
    private EmitBulletAbility emitBulletAbility;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        emitBulletAbility.timer.Timeout += OnEmitBulletAbilityReady;

        emitBulletAbility.enable = true;
        emitBulletAbility.SetTargetNode(GetTree().GetFirstNodeInGroup<Node2D>(GlobalExport.Instance.onGround));
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    private void OnEmitBulletAbilityReady()
    {
        emitBulletAbility.EmitBullet(bulleScene);
    }
}
