using Godot;
using GodotUtilities;

[Scene]
public partial class Lane : Control, IPosition
{
    [Export]
    public Corner relativeCorner { get; private set; } = Corner.RightBottom;

    [Export]
    public Vector2 offsetPosition { get; private set; } = Vector2.Zero;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        SignalBus.Instance.EmitSignal(SignalBus.SignalName.ZombieProducePositionReady, this);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public Vector2 GetPosition()
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
