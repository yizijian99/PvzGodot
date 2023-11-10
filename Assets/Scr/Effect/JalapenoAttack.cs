using Godot;
using GodotUtilities;

namespace Pvz.Assets.Scr.Effect;

[Scene]
public partial class JalapenoAttack : Node2D
{
	[Export]
	private AnimatedSprite2D left;

	[Export]
	private AnimatedSprite2D right;

	private int state;
	
	public override void _Ready()
	{
		base._Ready();
		WireNodes();

		left.AnimationFinished += () =>
		{
			state++;
		};

		right.AnimationFinished += () =>
		{
			state++;
		};
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (state == 3)
		{
			return;
		}
		if (state == 2)
		{
			state++;
			CallDeferred(Node.MethodName.QueueFree);
		}
	}
}