using Godot;
using GodotUtilities;

[Scene]
public partial class GlobalExport : Node
{
	public static GlobalExport Instance { get; private set; }

	#region Export
	[Export]
	public PackedScene sun { get; private set; }
    #endregion

    public override void _Ready()
	{
		base._Ready();
		WireNodes();
		Instance = this;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
