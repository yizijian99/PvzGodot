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
        if (InputUtils.MouseLeftButtonPressed(@event))
        {
            SignalBus.Instance.EmitSignal(SignalBus.SignalName.Ground_GridBeClicked, this);
        }
    }

    public void Plant(Card card)
    {
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
