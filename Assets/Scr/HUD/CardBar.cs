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

    private int length;

    public int Length => length;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        hBoxContainer.ChildOrderChanged += GetLength;
        GetLength();
    }

    private void GetLength()
    {
        length = hBoxContainer.GetChildrenOfType<BaseCard>().Count();
    }

    public bool Add(BaseCard card)
    {
        if (Length >= MaxCapacity)
        {
            return false;
        }

        hBoxContainer.AddChild(card);
        return true;
    }

    public void Remove(BaseCard card)
    {
        hBoxContainer.RemoveChild(card);
        card.QueueFree();
    }

    public void EnterCombatStage()
    {
        foreach (BaseCard card in hBoxContainer.GetChildrenOfType<BaseCard>())
        {
            card.State = BaseCard.CardState.Combat;
        }
    }
}