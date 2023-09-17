using Godot;
using GodotUtilities;
using Pvz.Assets.Scr.Autoload;

namespace Pvz.Assets.Scr.Ability;

[Scene]
public partial class ProduceSunAbility : Node
{
    [Export(PropertyHint.File, "*.tscn")]
    private string defaultSunScenePath;

    [Export]
    private CollisionShape2D sunOutlet;

    [Export]
    private Node2D ground;

    [Node("Timer")]
    private Timer timer;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        timer.Timeout += OnTimerTimeout;
        SignalBus.Instance.MainGameStarted += Start;
    }

    public void Start()
    {
        timer.Start();
    }

    public void Stop()
    {
        timer.Stop();
    }

    private void OnTimerTimeout()
    {
        ProduceSun();
    }

    public void ProduceSun(string sunScenePath = null)
    {
        if (sunScenePath == null)
        {
            sunScenePath = defaultSunScenePath;
        }

        Node2D sun = ResourceLoader.Load<PackedScene>(sunScenePath)?.InstantiateOrNull<Node2D>();
        if (sun == null)
        {
            return;
        }

        Rect2 rect2 = sunOutlet.Shape.GetRect();
        RandomNumberGenerator rng = new();
        float randomX = sunOutlet.GlobalPosition.X - rect2.Size.X / 2 + rng.RandfRange(0, rect2.Size.X);
        float randomY = sunOutlet.GlobalPosition.Y - rect2.Size.Y / 2 + rng.RandfRange(0, rect2.Size.Y);
        Vector2 randomGlobalPosition = new(randomX, randomY);

        sun.GlobalPosition = randomGlobalPosition;
        ground.AddChild(sun);
    }
}