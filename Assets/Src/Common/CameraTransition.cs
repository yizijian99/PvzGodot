using Godot;
using GodotUtilities;

[Scene]
public partial class CameraTransition : Node
{
    [Node("Camera2D")]
    private Camera2D camera2D;  // ȫ�������

    public static CameraTransition Instance { get; private set; }

    private bool transitioning = false;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();
        Instance = this;

        // �ر�ȫ�����������ֹ����Ҫ������ĳ����ܵ�ȫ���������Ӱ��
        camera2D.Enabled = false;
    }

    public void TransitionCamera2D(Camera2D from, Camera2D to, float duration = 1f)
    {
        if (transitioning) return;

        // ����ȫ�������
        camera2D.Enabled = true;

        // ������ʼ�����������
        camera2D.Zoom = from.Zoom;
        camera2D.Offset = from.Offset;
        camera2D.LightMask = from.LightMask;

        // ��ȫ��������ƶ�����ʼ�������λ��
        camera2D.GlobalTransform = from.GlobalTransform;

        // ����ȫ�������Ϊ��ǰ�����
        camera2D.MakeCurrent();

        transitioning = true;

        // �������䶯��
        Tween tween = CreateTween();
        // �ƶ�ȫ���������Ŀ���������λ��
        tween.SetTrans(Tween.TransitionType.Cubic);
        tween.SetEase(Tween.EaseType.InOut);
        tween.TweenProperty(camera2D, "global_transform", to.GlobalTransform, duration);
        tween.TweenProperty(camera2D, "zoom", to.Zoom, duration);
        tween.TweenProperty(camera2D, "offset", to.Offset, duration);
        // �ȴ����䶯�����
        tween.TweenCallback(Callable.From(() =>
        {
            // ����Ŀ�������Ϊ��ǰ�����
            to.MakeCurrent();

            transitioning = false;

            // �ر�ȫ�������
            camera2D.Enabled = false;
        }));
    }
}
