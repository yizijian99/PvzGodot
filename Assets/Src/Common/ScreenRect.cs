using Godot;
using GodotUtilities;

[Scene]
public partial class ScreenRect : CanvasLayer
{
    public static ScreenRect Instance { get; private set; }

    [Node("Control")]
    private Control control;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();
        Instance = this;
        Visible = false;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public void Enable()
    {
        Visible = true;
    }

    public void Disable()
    {
        Visible = false;
    }
}
