using Godot;
using GodotUtilities;

[Scene]
public partial class StageReminder : TextureRect
{
    [Export]
    private Texture2D startReady;

    [Export]
    private Texture2D startSet;

    [Export]
    private Texture2D startPlant;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public void PlayStartAnimation()
    {
        Tween tween = CreateTween();
        tween.TweenInterval(0.5);
        tween.TweenCallback(Callable.From(() => this.Texture = startReady));
        tween.TweenInterval(0.5);
        tween.TweenCallback(Callable.From(() => this.Texture = startSet));
        tween.TweenInterval(0.5);
        tween.TweenCallback(Callable.From(() => this.Texture = startPlant));
        tween.TweenInterval(0.5);
        tween.TweenCallback(Callable.From(() => this.Texture = null));
    }
}
