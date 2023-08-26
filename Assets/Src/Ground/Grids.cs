using Godot;
using GodotUtilities;

[Scene]
public partial class Grids : Control
{
    [Node("Preview")]
    private Sprite2D preview;

    private Grid focusGrid;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        SignalBus.Instance.Game_SelectedCardChanged += (oldValue, newValue) => preview.Texture = newValue?.preview;
        SignalBus.Instance.Ground_GridMouseEntered += OnGridMouseEntered;
        SignalBus.Instance.Ground_GridMouseExited += OnGridMouseExited;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    private void OnGridMouseEntered(Grid grid)
    {
        focusGrid = grid;
        if (preview == null)
            return;
        preview.Visible = focusGrid.isAvailable();
        preview.GlobalPosition = focusGrid.GlobalCenter();
    }

    private void OnGridMouseExited(Grid grid)
    {
        focusGrid = null;
        if (preview == null)
            return;
        preview.Visible = false;
    }
}
