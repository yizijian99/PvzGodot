using Godot;
using GodotUtilities;

[Scene]
public partial class SunFactory : Area2D
{
    #region Export
    [Export]
    private Node targetNode;
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

    public void GenerateSun()
    {
        Rect2 rect = collisionShape2D.Shape.GetRect();

        RandomNumberGenerator rng = new RandomNumberGenerator();
        float x = rng.RandfRange(rect.Position.X, rect.End.X);
        float y = rng.RandfRange(rect.Position.Y, rect.End.Y);

        Node2D sun = GlobalExport.Instance.sun.Instantiate<Node2D>();
        sun.Position = new Vector2(x, y);
        targetNode.AddChild(sun);
    }
}
