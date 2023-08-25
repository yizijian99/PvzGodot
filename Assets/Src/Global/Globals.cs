using Godot;
using GodotUtilities;

[Scene]
public partial class Globals : Node
{
    public static Globals Instance { get; private set; }

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
