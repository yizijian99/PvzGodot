using Godot;
using GodotUtilities;

[Scene]
public partial class ProduceZombieAbility : Node
{
	public override void _Ready()
	{
		base._Ready();
		WireNodes();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
