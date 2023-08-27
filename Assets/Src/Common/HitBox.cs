using Godot;
using GodotUtilities;

[Tool]
[Scene]
[GlobalClass]
public partial class HitBox : Area2D, CollisionBox
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

        AreaEntered += area =>
        {
            if (area is CollisionBox hurtBox)
            {
                HitHandler hurtOwner = hurtBox.GetCollisionBoxOwner();
                HitHandler hitOwner = GetCollisionBoxOwner();
                if (hitOwner == null || hurtOwner == null)
                    return;
                HitRequest request = hitOwner.buildHitRequest();
                if (request == null)
                    return;
                HitResponse response = new HitResponse();
                hurtOwner.DoHitRequest(request, response);
                hitOwner.DoHitResponse(response);
            }
        };
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
