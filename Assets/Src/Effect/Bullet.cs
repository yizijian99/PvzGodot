using Godot;
using GodotUtilities;

[Scene]
public partial class Bullet : RigidBody2D
{
	[Node("VisibleOnScreenNotifier2D")]
	private VisibleOnScreenNotifier2D visibleOnScreenNotifier;

    public override void _Ready()
	{
		base._Ready();
		WireNodes();

		visibleOnScreenNotifier.ScreenExited += () => QueueFree();
    }

	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
