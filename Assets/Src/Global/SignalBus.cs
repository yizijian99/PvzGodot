using Godot;
using GodotUtilities;

[Scene]
public partial class SignalBus : Node
{
	public static SignalBus Instance { get; private set; }

    #region Signal
    [Signal]
	public delegate void SunPickedEventHandler(int amount);

    [Signal]
    public delegate void GameTotalSunChangedEventHandler(int amount);

    [Signal]
    public delegate void CardNodeReadyEventHandler();

    [Signal]
    public delegate void CardSelectedEventHandler(Card card);

    [Signal]
    public delegate void CardCostSunEventHandler(Card card);

    [Signal]
    public delegate void GroundGridClickedEventHandler(Grid grid);
    #endregion

    public override void _Ready()
	{
		base._Ready();
		WireNodes();
		Instance = this;

        #region Signal
        #endregion
    }

    public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
