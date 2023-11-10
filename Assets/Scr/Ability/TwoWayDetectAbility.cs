using System.Collections.Generic;
using Godot;

namespace Pvz.Assets.Scr.Ability;

[GlobalClass]
public partial class TwoWayDetectAbility : BaseDetectAbility
{
    [Export]
    public RayCast2D Detector1 { get; set; }

    [Export]
    public RayCast2D Detector2 { get; set; }

    public override void _Process(double delta)
    {
        base._Process(delta);
        List<GodotObject> targets = Detect();
        if (targets.Count > 0)
        {
            EmitSignal(BaseDetectAbility.SignalName.Detected, targets[0]);
        }
    }

    public override List<GodotObject> Detect()
    {
        List<GodotObject> list = new();
        GodotObject o1 = Detector1.GetCollider();
        if (o1 != null)
        {
            list.Add(o1);
        }

        GodotObject o2 = Detector2.GetCollider();
        if (o2 != null)
        {
            list.Add(o2);
        }

        return list;
    }
}