using Godot;
using GodotUtilities;

[Scene]
public partial class EmitBulletAbility : Node
{
    [Export]
    private bool enable;
    public bool Enable
    {
        get => enable;
        set
        {
            enable = value;
            if (timer != null)
            {
                if (enable)
                    timer.Start();
                else
                    timer.Stop();
            }
        }
    }

    [Export]
    private CandidateArea emitArea;

    [Node("Timer")]
    public Timer timer { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        timer.Autostart = false;
        if (enable) timer.Start();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public bool CanEmit()
    {
        return enable && timer.IsStopped();
    }

    public void EmitBullet(PackedScene bulletScene)
    {
        Bullet bullet = bulletScene.InstantiateOrNull<Bullet>();
        if (bullet == null)
        {
            return;
        }
        if (!CanEmit())
        {
            return;
        }
        emitArea.AddChildToRandomPosition(bullet);
        timer.Start();
    }

    public void SetTargetNode(Node2D targetNode)
    {
        emitArea.targetNode = targetNode;
    }
}
