using Godot;
using GodotUtilities;
using Pvz.Assets.Scr.Autoload;

namespace Pvz.Assets.Scr.Card;

[Scene]
public partial class BaseCard : TextureButton
{
    [Export]
    public Texture2D Texture { get; protected set; }

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
                        if (CoolDownTime > 0)
                        {
                            Timer.WaitTime = CoolDownTime;
                            if (NeedToWait)
                            {
                                Timer.Start();
                            }
                        }

                        break;
                }
            }
        }
    }

    [Export]
    public float CoolDownTime { get; protected set; }

    [Export]
    public bool NeedToWait { get; protected set; } = true;

    [Export]
    public int Cost { get; protected set; }

    [Export(PropertyHint.File, "*.tscn")]
    public string EntityScenePath { get; protected set; }

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
        if (string.IsNullOrEmpty(EntityScenePath))
        {
            Disabled = true;
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (State == CardState.Combat)
        {
            CoolDownMask.Value = CoolDownMask.MinValue +
                                 (CoolDownMask.MaxValue - CoolDownMask.MinValue) * (Timer.TimeLeft / Timer.WaitTime);
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
            case CardState.Combat:
                ReadyToPlant();
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

    public void ReadyToPlant()
    {
        if (Timer.IsStopped())
        {
            SignalBus.Instance.EmitSignal(SignalBus.SignalName.CardReadyToPlant, this);
        }
    }

    public void CoolDown()
    {
        Timer.Start();
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