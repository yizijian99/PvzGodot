using Godot;
using GodotUtilities;
using Pvz.Assets.Scr.Autoload;

namespace Pvz.Assets.Scr.Card;

[Scene]
public partial class BaseCard : TextureButton
{
    private CardState state;

    [Export]
    public CardState State
    {
        get => state;
        set
        {
            CardState oldState = state;
            state = value;
            if (oldState != state)
            {
                switch (state)
                {
                    case CardState.Combat:
                        if (NeedToWait && CoolDownTime > 0)
                        {
                            Timer.WaitTime = CoolDownTime;
                            Timer.Start();
                        }

                        break;
                }
            }
        }
    }

    [Export]
    public float CoolDownTime { get; private set; }

    [Export]
    public bool NeedToWait { get; private set; } = true;
    
    [Export]
    public int Cost { get; protected set; }

    [Node("DisableMask")]
    protected ColorRect DisableMask;

    [Node("CoolDownMask")]
    protected TextureProgressBar CoolDownMask;

    [Node("Timer")]
    protected Timer Timer;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        Pressed += OnCardPressed;
        SignalBus.Instance.SunCountChanged += OnSunCountChanged;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (State == CardState.Combat)
        {
            CoolDownMask.Value = CoolDownMask.MinValue + (CoolDownMask.MaxValue - CoolDownMask.MinValue) * (Timer.TimeLeft / Timer.WaitTime);
        }
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

    private void OnSunCountChanged(int value)
    {
        switch (State)
        {
            case CardState.Combat:
                DisableMask.Visible = value < Cost;
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
            DisableMask.Visible = value;
        }
    }

    public enum CardState
    {
        Default,
        Candidate,
        ReadyToCombat,
        Combat
    }
}