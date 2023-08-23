using Godot;
using GodotUtilities;

[Scene]
public partial class ProduceSunAbility : Node
{

	#region Export
	[Export]
	private SunFactory sunFactory;
	#endregion

	#region Node
	[Node("Timer")]
	private Timer timer;
    #endregion

    public override void _Ready()
	{
		base._Ready();
		WireNodes();

		#region Signal
		timer.Timeout += OnTimerTimeout;
        #endregion
    }

    public override void _Process(double delta)
	{
		base._Process(delta);
	}

	private void OnTimerTimeout()
	{
		sunFactory.ProduceSun();
    }
}
