using Godot;
using GodotUtilities;

[Scene]
public partial class SunFactory : Area2D
{
    #region Export
    [Export]
    private Node targetNode;

    [Export]
    private Vector2 minInitSpeed = Vector2.Zero;

    [Export]
    private Vector2 maxInitSpeed = Vector2.Zero;

    [Export]
    private float gravityScale = 0f;
    #endregion

    #region Node
    [Node("CollisionShape2D")]
    private CollisionShape2D collisionShape2D;
    #endregion

    public override void _Ready()
    {
        base._Ready();
        WireNodes();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public void ProduceSun()
    {
        Rect2 rect = collisionShape2D.Shape.GetRect();

        RandomNumberGenerator rng = new RandomNumberGenerator();
        float x = rng.RandfRange(rect.Position.X, rect.End.X);
        float y = rng.RandfRange(rect.Position.Y, rect.End.Y);

        Sun sun = GlobalExport.Instance.sun.Instantiate<Sun>();
        sun.Position = new Vector2(x, y);
        sun.ApplyImpulse(new Vector2(rng.RandfRange(minInitSpeed.X, maxInitSpeed.X), rng.RandfRange(minInitSpeed.Y, maxInitSpeed.Y)));
        sun.GravityScale = gravityScale;
        targetNode.AddChild(sun);
    }
}
