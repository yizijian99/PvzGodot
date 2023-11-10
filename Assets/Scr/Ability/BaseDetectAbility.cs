using System.Collections.Generic;
using Godot;
using Pvz.Assets.Scr.Zombies;

namespace Pvz.Assets.Scr.Ability;

[GlobalClass]
public partial class BaseDetectAbility : Node2D
{
    [Signal]
    public delegate void DetectedEventHandler(Zombie zombie);

    public virtual List<GodotObject> Detect()
    {
        return new List<GodotObject>();
    }

    public virtual void Enable()
    {
        
    }

    public virtual void Disable()
    {
        
    }
}