using Godot;
using GodotUtilities;
using Pvz.Assets.Scr.Effect;

namespace Pvz.Assets.Scr.Plant;

[Scene]
public partial class PlantJalapeno : BasePlant
{
    [Export(PropertyHint.File, "*.tscn")]
    private string jalapenoAttackScenePath;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        AnimatedSprite2D.AnimationFinished += () =>
        {
            if (AnimatedSprite2D.Animation == "default")
            {
                Attack();
            }
        };
    }

    public void Attack()
    {
        JalapenoAttack jalapenoAttack = ResourceLoader.Load<PackedScene>(jalapenoAttackScenePath)?.InstantiateOrNull<JalapenoAttack>();
        if (jalapenoAttack != null)
        {
            jalapenoAttack.GlobalPosition = GlobalPosition;
            GetParent().AddChildDeferred(jalapenoAttack);
        }

        CallDeferred(Node.MethodName.QueueFree);
    }
}