using Godot;
using GodotUtilities;

[Tool]
[Scene]
[GlobalClass]
public partial class HurtBox : Area2D, CollisionBox
{

    [Export]
    public Node2D collisionBoxOwner;

    [Export]
    public CollisionShape2D collider;

    [Export]
    public bool enable
    {
        get => collider == null ? false : !collider.Disabled;
        set
        {
            if (collider == null)
                return;
            collider.Disabled = !value;
        }
    }

    public override void _Ready()
    {
        base._Ready();
        WireNodes();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public HitHandler GetCollisionBoxOwner()
    {
        if (collisionBoxOwner is HitHandler)
        {
            return collisionBoxOwner as HitHandler;
        }
        return null;
    }
}
