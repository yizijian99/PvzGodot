using System;
using Godot;
using GodotUtilities;

[Scene]
public partial class CameraTransition : Node
{
    [Node("Camera2D")]
    private Camera2D camera2D;  // 全局摄像机

    public static CameraTransition Instance { get; private set; }

    private bool transitioning = false;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();
        Instance = this;

        // 关闭全局摄像机，防止不需要摄像机的场景受到全局摄像机的影响
        camera2D.Enabled = false;
    }

    public async void TransitionCamera2D(Camera2D from, Camera2D to, float duration = 1f, Action action = null)
    {
        if (transitioning) return;

        transitioning = true;

        // 开启全局摄像机
        camera2D.Enabled = true;

        // 复制起始摄像机的属性
        camera2D.Zoom = from.Zoom;
        camera2D.Offset = from.Offset;
        camera2D.LightMask = from.LightMask;

        // 将全局摄像机移动到起始摄像机的位置
        camera2D.GlobalTransform = from.GlobalTransform;

        // 设置全局摄像机为当前摄像机
        camera2D.MakeCurrent();

        // 创建补间动画
        Tween tween = CreateTween();
        // 移动全局摄像机到目标摄像机的位置
        tween.SetTrans(Tween.TransitionType.Cubic);
        tween.SetEase(Tween.EaseType.InOut);
        tween.SetParallel();
        tween.TweenProperty(camera2D, "global_transform", to.GlobalTransform, duration);
        tween.TweenProperty(camera2D, "zoom", to.Zoom, duration);
        Tweener tweener = tween.TweenProperty(camera2D, "offset", to.Offset, duration);
        // 等待补间动画完成
        await ToSignal(tweener, Tweener.SignalName.Finished);

        // 设置目标摄像机为当前摄像机
        to.MakeCurrent();

        // 关闭全局摄像机
        camera2D.Enabled = false;

        transitioning = false;

        if (action != null)
        {
            action.Invoke();
        }
    }
}
