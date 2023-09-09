using Godot;
using Godot.Collections;
using GodotUtilities;

[Scene]
public partial class StageReminder : TextureRect
{

    [Export]
    private Array<Texture2D> texture2DList;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public async void PlayStartAnimation()
    {
        Tween tween = CreateTween();
        foreach (Texture2D item in texture2DList)
        {
            tween.TweenInterval(0.5);
            tween.TweenCallback(Callable.From(() => this.Texture = item));
        }
        tween.TweenInterval(0.5);
        tween.TweenCallback(Callable.From(() => this.Texture = null));
        await ToSignal(tween, Tween.SignalName.Finished);
        ScreenRect.Instance.Disable();
    }
}
