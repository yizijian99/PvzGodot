using Godot;
using GodotUtilities;
using Pvz.Assets.Scr.Card;
using Pvz.Assets.Scr.Effect;

namespace Pvz.Assets.Scr.Autoload;

[Scene]
public partial class SignalBus : Node
{
	public static SignalBus Instance { get; private set; }

	public override void _Ready()
	{
		base._Ready();
		WireNodes();
		Instance = this;
	}

	[Signal]
	public delegate void CardToCombatEventHandler(BaseCard card);

	[Signal]
	public delegate void CardToCandidateEventHandler(BaseCard card);

	[Signal]
	public delegate void MainGameStartedEventHandler();

	[Signal]
	public delegate void SunPickedEventHandler(Sun sun);

	[Signal]
	public delegate void SunCountChangedEventHandler(int value);
}