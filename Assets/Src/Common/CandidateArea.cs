using Godot;
using GodotUtilities;

[GlobalClass]
[Scene]
public partial class CandidateArea : Node2D
{
	[Export]
	private Shape2D shape;

	[Export]
	public Node2D targetNode;

    public override void _Ready()
	{
		base._Ready();
		WireNodes();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public Vector2 RandomPositon()
	{
		Rect2 rect = shape.GetRect();
        RandomNumberGenerator rng = new RandomNumberGenerator();
        float x = rng.RandfRange(rect.Position.X, rect.End.X);
        float y = rng.RandfRange(rect.Position.Y, rect.End.Y);
		return ToGlobal(new Vector2(x, y));
    }

    public void AddChildToRandomPosition(Node2D addedNode)
    {
        if (targetNode == null) targetNode = this;
        addedNode.GlobalPosition = RandomPositon();
        targetNode.AddChild(addedNode);
    }
}
