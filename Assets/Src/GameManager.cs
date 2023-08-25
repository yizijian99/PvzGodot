using Godot;
using GodotUtilities;

[Scene]
public partial class GameManager : Node
{
    [Export]
    private int _totalSuns = 50;
    public int totalSuns
    {
        get => _totalSuns;
        set
        {
            int oldValue = _totalSuns;
            int newValue = value;
            bool totalSunsChanged = oldValue != newValue;
            _totalSuns = newValue;
            if (totalSunsChanged)
                SignalBus.Instance.EmitSignal(SignalBus.SignalName.Game_TotalSunsChanged, oldValue, newValue);
        }
    }

    private Card selectedCard;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        SignalBus.Instance.Sun_PickUp += amount => totalSuns += amount;
        SignalBus.Instance.Card_NodeReady += () => SignalBus.Instance.EmitSignal(SignalBus.SignalName.Game_TotalSunsChanged, _totalSuns, _totalSuns);
        SignalBus.Instance.Ground_GridBeClicked += OnGroundGridBeClicked;
        SignalBus.Instance.Card_BeClicked += OnCardBeClicked;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        InputUtils.SetCustomMouseCursor(selectedCard?.preview);
    }

    private void OnGroundGridBeClicked(Grid grid)
    {
        if (grid == null) return;
        if (selectedCard == null) return;
        if (selectedCard.cost > totalSuns)
        {
            return;
        }
        bool planted = grid.Plant(selectedCard);
        if (!planted)
        {
            return;
        }
        totalSuns -= selectedCard.cost;
        selectedCard.CoolDown();
        selectedCard = null;
    }

    private void OnCardBeClicked(Card card)
    {
        if (selectedCard == card)
        {
            selectedCard = null;
            return;
        }
        selectedCard = card;
    }
}
