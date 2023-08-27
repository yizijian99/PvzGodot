using Godot;
using GodotUtilities;

[Scene]
public partial class EmitBulletAbility : Node
{
    private bool _enable;
    [Export]
    public bool enable
    {
        get => _enable;
        set
        {
            _enable = value;
            if (timer != null)
            {
                if (_enable)
                {
                    EmitBullet();
                    timer.Start();
                }
                else
                    timer.Stop();
            }
        }
    }

    [Export]
    private PackedScene bulletScene;

    [Export]
    private CandidateArea emitArea;

    [Node("Timer")]
    public Timer timer { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        timer.Autostart = false;
        if (_enable) timer.Start();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public bool CanEmit()
    {
        return _enable && timer.IsStopped();
    }

    public void EmitBullet()
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
