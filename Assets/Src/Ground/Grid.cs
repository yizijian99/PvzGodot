using Godot;
using GodotUtilities;

[Scene]
public partial class Grid : Control
{
    [Export]
    private Node2D ground;

	public override void _Ready()
	{
		base._Ready();
		WireNodes();

        GuiInput += OnGuiInput;
    }

	public override void _Process(double delta)
	{
		base._Process(delta);
	}

    private void OnGuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                SignalBus.Instance.EmitSignal(SignalBus.SignalName.GroundGridClicked, this);
            }
        }
    }

    public void Plant(Card card)
    {
        GD.Print($"{card.Name} {GlobalPosition}");
        card.Consume();
        Plant plant = card.plantScene.Instantiate<Plant>();
        plant.GlobalPosition = GlobalCenter(); ;
        ground.AddChild(plant);
    }

    public Vector2 LocalCenter()
    {
        return Position + Size / 2;
    }
    public Vector2 GlobalCenter()
    {
        return GlobalPosition + Size / 2;
    }
}
