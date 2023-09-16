using System.Linq;
using Godot;
using GodotUtilities;
using Pvz.Assets.Scr.Card;

namespace Pvz.Assets.Scr.HUD;

[Scene]
public partial class CardBar : TextureRect
{
    [Node("HBoxContainer")]
    private HBoxContainer hBoxContainer;

    public int MaxCapacity { get; private set; } = 8;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();
    }

    public int Length()
    {
        return hBoxContainer.GetChildrenOfType<BaseCard>().Count();
    }

    public bool Add(BaseCard card)
    {
        if (Length() >= MaxCapacity)
        {
            return false;
        }

        hBoxContainer.AddChild(card);
        return true;
    }

    public void Remove(BaseCard card)
    {
        hBoxContainer.RemoveChild(card);
    }
}