using Godot;
using GodotUtilities;

[Scene]
public partial class Bullet : RigidBody2D, HitHandler
{
    [Node("VisibleOnScreenNotifier2D")]
    private VisibleOnScreenNotifier2D visibleOnScreenNotifier;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        visibleOnScreenNotifier.ScreenExited += Distroy;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public HitRequest buildHitRequest()
    {
        HitRequest request = new HitRequest();
        request.damage = 10;
        return request;
    }

    public void DoHitRequest(HitRequest request, HitResponse response)
    {

    }

    public void DoHitResponse(HitResponse response)
    {
        if (response.isOk())
        {
            Distroy();
        }
    }

    public void Distroy()
    {
        GD.Print("bullet destroyed");
        QueueFree();
    }
}
