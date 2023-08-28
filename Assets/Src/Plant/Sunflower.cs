using GodotUtilities;

[Scene]
public partial class Sunflower : Plant
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
