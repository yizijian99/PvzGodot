using Godot;
using GodotUtilities;
using Pvz.Assets.Scr.Autoload;

namespace Pvz.Assets.Scr.Card;

[Scene]
public partial class BaseCard : TextureButton
{
    [Node("DisableMask")]
    private ColorRect disableMask;

    [Export]
    public CardState State { get; set; } = CardState.Default;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        Pressed += OnCardPressed;
    }

    protected virtual void OnCardPressed()
    {
        switch (State)
        {
            case CardState.Candidate:
                ToCombat();
                break;
            case CardState.ReadyToCombat:
                ToCandidate();
                break;
        }
    }

    public void ToCombat()
    {
        SignalBus.Instance.EmitSignal(SignalBus.SignalName.CardToCombat, this);
    }

    public void ToCandidate()
    {
        SignalBus.Instance.EmitSignal(SignalBus.SignalName.CardToCandidate, this);
    }

    public new bool Disabled
    {
        get => base.Disabled;
        set
        {
            base.Disabled = value;
            disableMask.Visible = value;
        }
    }

    public enum CardState
    {
        Default,
        Candidate,
        ReadyToCombat
    }
}