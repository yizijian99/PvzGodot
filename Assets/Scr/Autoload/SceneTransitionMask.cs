using Godot;

namespace Pvz.Assets.Scr.Autoload;

public partial class SceneTransitionMask : CanvasLayer
{
	public static SceneTransitionMask Instance { get; private set; }

	private Control control;

	public override void _Ready()
	{
		base._Ready();
		Instance = this;
		control = GetNode<Control>("Control");

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