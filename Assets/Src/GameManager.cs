using Godot;
using GodotUtilities;

[Scene]
public partial class GameManager : Node
{
    [Export]
    private int totalSun = 50;
    public int TotalSun
    {
        get => totalSun;
        set
        {
            bool totalSunChanged = totalSun != value;
            totalSun = value;
            SyncTotalToLabel();
            if (totalSunChanged)
                EventBus.Instance.EmitSignal(EventBus.SignalName.GameTotalSunChanged, totalSun);
        }
    }


    [Export]
    private Label totalSunLabel;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        EventBus.Instance.SunPicked += amount => TotalSun += amount;
        EventBus.Instance.CardNodeReady += () => EventBus.Instance.EmitSignal(EventBus.SignalName.GameTotalSunChanged, totalSun);
        EventBus.Instance.CardCostSun += card =>
        {
            if (totalSun < card.cost) return;
            card.Consume();
            TotalSun -= card.cost;
        };

        SyncTotalToLabel();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    private void SyncTotalToLabel()
    {
        if (totalSunLabel == null)
        {
            return;
        }
        totalSunLabel.Text = totalSun.ToString();
    }
}
