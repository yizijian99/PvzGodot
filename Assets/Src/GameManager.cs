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
                SignalBus.Instance.EmitSignal(SignalBus.SignalName.GameTotalSunChanged, totalSun);
        }
    }


    [Export]
    private Label totalSunLabel;

    private Card selectedCard;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        SignalBus.Instance.SunPicked += amount => TotalSun += amount;
        SignalBus.Instance.CardNodeReady += () => SignalBus.Instance.EmitSignal(SignalBus.SignalName.GameTotalSunChanged, totalSun);
        SignalBus.Instance.CardCostSun += card =>
        {
            if (totalSun < card.cost) return;
            TotalSun -= card.cost;
            selectedCard = null;
            card.Cooldown();
        };
        SignalBus.Instance.GroundGridClicked += OnGridClicked;
        SignalBus.Instance.CardSelected += OnCardSelected;

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

    private void OnGridClicked(Grid grid)
    {
        if (grid == null) return;
        if (selectedCard == null) return;
        grid.Plant(selectedCard);
    }

    private void OnCardSelected(Card card)
    {
        selectedCard = card;
    }
}
