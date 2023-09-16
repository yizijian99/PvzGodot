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

        hBoxContainer.ChildEnteredTree += OnHBoxContainerChildEnteredTree;
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
        card.TreeExited += GetLength;
        hBoxContainer.RemoveChild(card);
        card.QueueFree();
    }

    public void OnHBoxContainerChildEnteredTree(Node node)
    {
        if (node is BaseCard)
        {
            GetLength();
        }
    }

    public void EnterCombatStage()
    {
        foreach (BaseCard card in hBoxContainer.GetChildrenOfType<BaseCard>())
        {
            card.State = BaseCard.CardState.Combat;
        }
    }
}