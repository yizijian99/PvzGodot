using Godot;
using GodotUtilities;

[Scene]
public partial class ProduceSunAbility : Node
{

	[Export]
	private SunFactory sunFactory;

	[Node("Timer")]
	private Timer timer;

    public override void _Ready()
	{
		base._Ready();
		WireNodes();

		timer.Timeout += OnTimerTimeout;
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
