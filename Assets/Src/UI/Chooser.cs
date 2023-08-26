using Godot;
using GodotUtilities;

[Scene]
public partial class Chooser : Control
{
    [Node("TotalSuns")]
    private Label totalSunsLabel;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        SignalBus.Instance.Game_TotalSunsChanged += OnGameTotalSunsChanged;

        SignalBus.Instance.EmitSignal(SignalBus.SignalName.TotalSunsLabel_NodeReady);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    private void OnGameTotalSunsChanged(int oldValue, int newValue)
    {
        totalSunsLabel.Text = newValue.ToString();
    }
}
