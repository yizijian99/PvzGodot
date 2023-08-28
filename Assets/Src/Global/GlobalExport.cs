using Godot;
using GodotUtilities;

[Scene]
public partial class GlobalExport : Node
{
    public static GlobalExport Instance { get; private set; }

    [Export]
    public PackedScene sun { get; private set; }

    [Export]
    public PackedScene zombie { get; private set; }

    [Export]
    public string onGround { get; private set; } = "onGround";

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
