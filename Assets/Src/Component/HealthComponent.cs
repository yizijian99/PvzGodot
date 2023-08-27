using Godot;
using GodotUtilities;

[Scene]
public partial class HealthComponent : Node
{
    [Signal]
    public delegate void ZeroHealthEventHandler();

    private int _health;
    [Export]
    public int health
    {
        get => _health;
        set
        {
            int oldValue = _health;
            int newValue = value;
            _health = newValue;
            if (oldValue != newValue)
            {
                if (_health <= 0)
                {
                    EmitSignal(SignalName.ZeroHealth);
                }
            }
        }
    }

    public override void _Ready()
    {
        base._Ready();
        WireNodes();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }
}
