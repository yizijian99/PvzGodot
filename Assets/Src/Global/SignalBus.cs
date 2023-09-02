using Godot;
using GodotUtilities;

[Scene]
public partial class SignalBus : Node
{
    public static SignalBus Instance { get; private set; }

    [Signal]
    public delegate void Sun_PickUpEventHandler(int value);

    [Signal]
    public delegate void Game_TotalSunsChangedEventHandler(int oldValue, int newValue);

    [Signal]
    public delegate void Game_SelectedCardChangedEventHandler(Card oldValue, Card newValeu);

    [Signal]
    public delegate void TotalSunsLabel_NodeReadyEventHandler();

    [Signal]
    public delegate void Card_NodeReadyEventHandler();

    [Signal]
    public delegate void Card_BeClickedEventHandler(Card card);

    [Signal]
    public delegate void Ground_GridBeClickedEventHandler(Grid grid);

    [Signal]
    public delegate void Ground_GridMouseEnteredEventHandler(Grid grid);

    [Signal]
    public delegate void Ground_GridMouseExitedEventHandler(Grid grid);

    [Signal]
    public delegate void Plant_DeadEventHandler(Plant plant);

    [Signal]
    public delegate void ZombieProducePositionReadyEventHandler(GodotObject gdObj);

    public override void _Ready()
    {
        base._Ready();
        WireNodes();
        Instance = this;
    }
}
