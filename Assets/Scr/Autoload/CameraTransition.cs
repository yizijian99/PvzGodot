using System.Threading.Tasks;
using Godot;
using GodotUtilities;

namespace Pvz.Assets.Scr.Autoload;

[Scene]
public partial class CameraTransition : Node
{
    [Node("Camera2D")]
    private Camera2D globalCamera2D; // 全局摄像机

    public static CameraTransition Instance { get; private set; }

    private bool transitioning;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();
        Instance = this;

        // 关闭全局摄像机，防止不需要摄像机的场景受到全局摄像机的影响
        globalCamera2D.Enabled = false;
    }

    public async Task TransitionCamera2D(Camera2D camera, Node2D to, double duration = 1)
    {
        if (transitioning) return;

        transitioning = true;

        // 开启全局摄像机
        globalCamera2D.Enabled = true;

        // 复制起始摄像机的属性
        globalCamera2D.Zoom = camera.Zoom;
        globalCamera2D.Offset = camera.Offset;
        globalCamera2D.LightMask = camera.LightMask;

        // 将全局摄像机移动到起始摄像机的位置
        globalCamera2D.GlobalTransform = camera.GlobalTransform;

        // 设置全局摄像机为当前摄像机
        globalCamera2D.MakeCurrent();

        // 创建补间动画
        Tween tween = CreateTween();
        // 移动全局摄像机到目标摄像机的位置
        tween.SetTrans(Tween.TransitionType.Cubic);
        tween.SetEase(Tween.EaseType.InOut);
        tween.SetParallel();
        tween.TweenProperty(globalCamera2D, Node2D.PropertyName.GlobalTransform.ToString(), to.GlobalTransform, duration);
        // 等待补间动画完成
        await ToSignal(tween, Tween.SignalName.Finished);

        // 设置场景摄像机变换为全局摄像机变换
        camera.GlobalTransform = globalCamera2D.GlobalTransform;
        // 设置场景摄像机为当前摄像机
        camera.MakeCurrent();

        // 关闭全局摄像机
        globalCamera2D.Enabled = false;

        transitioning = false;
    }
}