using Godot;
using GodotUtilities;

[Scene]
public partial class Grid : Control
{
    [Export]
    private Node2D ground;

    private Plant plant;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        GuiInput += OnGuiInput;
        SignalBus.Instance.Plant_Dead += OnPlantDead;
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
        else if (InputUtils.MouseRightButtonPressed(@event))
        {
            Dig();
        }
    }

    private void OnPlantDead(Plant plant)
    {
        if (this.plant == plant)
        {
            this.plant = null;
        }
    }

    public bool Plant(Card card)
    {
        if (this.plant != null)
            return false;
        Plant plant = card.plantScene.Instantiate<Plant>();
        plant.GlobalPosition = GlobalCenter(); ;
        ground.AddChild(plant);
        this.plant = plant;
        return true;
    }

    public bool Dig()
    {
        if (this.plant == null)
        {
            return false;
        }
        plant.Dead();
        return true;
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
