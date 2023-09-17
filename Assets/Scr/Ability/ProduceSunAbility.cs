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
        float randomX = rng.RandfRange(rect2.Position.X, rect2.Size.X);
        float randomY = rng.RandfRange(rect2.Position.Y, rect2.Size.Y);
        Vector2 randomGlobalPosition = sunOutlet.ToGlobal(new Vector2(randomX, randomY));

        sun.GlobalPosition = randomGlobalPosition;
        ground.AddChild(sun);
    }
}