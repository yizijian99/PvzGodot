using System.Linq;
using Godot;
using Pvz.Assets.Scr.Autoload;
using Pvz.Assets.Scr.Card;

namespace Pvz.Assets.Scr.HUD;

public partial class CardBar : TextureRect
{
    private HBoxContainer hBoxContainer;

    public int MaxCapacity { get; private set; } = 8;

    private int length;

    public int Length => length;

    public override void _Ready()
    {
        base._Ready();
        hBoxContainer = GetNode<HBoxContainer>("HBoxContainer");

        hBoxContainer.ChildOrderChanged += GetLength;
        SignalBus.Instance.MainGameStarted += EnterCombatStage;
        
        GetLength();
    }

    private void GetLength()
    {
        length = hBoxContainer.GetChildren().Count(node => node is BaseCard);
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
        foreach (BaseCard card in hBoxContainer.GetChildren().Where(node => node is BaseCard).Cast<BaseCard>())
        {
            card.State = BaseCard.CardState.Combat;
        }
    }
}