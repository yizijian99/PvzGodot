using Godot;
using GodotUtilities;

[Scene]
public partial class Lane : Control
{
    [Export]
    public Corner relativeCorner = Corner.RightBottom;

    [Export]
    public Vector2 offsetPosition = Vector2.Zero;

    [Export]
    public Node2D ground;

    private Timer timer = new Timer();

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        timer.WaitTime = 3f;
        timer.OneShot = false;
        timer.Autostart = true;

        AddChild(timer);
        timer.Timeout += SpawnZombie;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public void SpawnZombie()
    {
        Zombie zombie = GlobalExport.Instance.zombie.Instantiate<Zombie>();
        ground.AddChild(zombie);
        zombie.GlobalPosition = GetRelativeCornerPosition();
    }

    public Vector2 GetRelativeCornerPosition()
    {
        Rect2 rect = GetGlobalRect();
        Vector2 cornerPosition = Vector2.Zero;
        switch (relativeCorner)
        {
            case Corner.RightTop:
                cornerPosition = rect.Position + new Vector2(rect.Size.X, 0);
                break;
            case Corner.RightBottom:
                cornerPosition = rect.Position + rect.Size;
                break;
            case Corner.LeftBottom:
                cornerPosition = rect.Position + new Vector2(0, rect.Size.Y);
                break;
            case Corner.LeftTop:
            default:
                cornerPosition = rect.Position;
                break;
        }
        return cornerPosition + offsetPosition;
    }
}
