using Godot;
using GodotUtilities;

[Scene]
public partial class Sun : RigidBody2D
{
	[Node("Control")]
	private Control control;

	public override void _Ready()
	{
		base._Ready();
		WireNodes();

		control.GuiInput += OnControlGuiInput;
    }

    public override void _Process(double delta)
	{
		base._Process(delta);
    }

    private void OnControlGuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                SignalBus.Instance.EmitSignal(SignalBus.SignalName.SunPicked, 25);
                QueueFree();
            }
        }
    }
}
