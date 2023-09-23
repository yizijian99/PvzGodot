using Godot;
using GodotUtilities;

namespace Pvz.Assets.Scr.Plant;

[Scene]
public partial class BasePlant : Node2D
{
	[Node("AnimatedSprite2D")]
	public AnimatedSprite2D AnimatedSprite2D { get; protected set; }
	
	public override void _Ready()
	{
		base._Ready();
		WireNodes();
	}
}