using Godot;
using GodotUtilities;

[Scene]
public partial class EventBus : Node
{
	public static EventBus Instance { get; private set; }

    #region Signal
    [Signal]
	public delegate void SunPickedEventHandler(int amount);

    [Signal]
    public delegate void GameTotalSunChangedEventHandler(int amount);

    [Signal]
    public delegate void CardNodeReadyEventHandler();

    [Signal]
    public delegate void CardCostSunEventHandler(Card card);
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
