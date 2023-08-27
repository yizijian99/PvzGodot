using Godot;
using GodotUtilities;

[Scene]
public partial class Plant : Node2D, HitHandler
{
    [Node("HealthComponent")]
    protected HealthComponent healthComponent;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        healthComponent.ZeroHealth += Dead;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public void Dead()
    {
        QueueFree();
        SignalBus.Instance.EmitSignal(SignalBus.SignalName.Plant_Dead, this);
    }

    public virtual HitRequest buildHitRequest()
    {
        return null;
    }

    public virtual void DoHitRequest(HitRequest request, HitResponse response)
    {
        healthComponent.health -= request.damage;
        GD.Print($"plant's health: {healthComponent.health}");
    }

    public virtual void DoHitResponse(HitResponse response)
    {

    }
}
