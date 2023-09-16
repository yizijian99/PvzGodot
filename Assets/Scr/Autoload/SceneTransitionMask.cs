using Godot;
using GodotUtilities;

namespace Pvz.Assets.Scr.Autoload;

[Scene]
public partial class SceneTransitionMask : CanvasLayer
{
	public static SceneTransitionMask Instance { get; private set; }

	[Node("Control")]
	private Control control;

	public override void _Ready()
	{
		base._Ready();
		WireNodes();
		Instance = this;

		Disable();
	}

	public void Enable()
	{
		control.Visible = true;
	}

	public void Disable()
	{
		control.Visible = false;
	}
}