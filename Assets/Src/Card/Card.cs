using Godot;
using GodotUtilities;

[Scene]
public partial class Card : TextureRect
{
    #region Nodes
    [Node("Timer")]
    private Timer timer;

    [Node("CostMask")]
    private ColorRect costMask;

    [Node("CooldownMask")]
    private TextureProgressBar cooldownMask;
    #endregion

    #region Export
    [Export]
    private string cardName;

    [Export]
    public PackedScene plantScene { get; private set; }

    [Export]
    public int cost { get; private set; }
    #endregion

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        GuiInput += OnGuiInput;
        timer.Timeout += OnTimerTimeOut;
        SignalBus.Instance.GameTotalSunChanged += OnGameTotalSunChanged;

        SignalBus.Instance.EmitSignal(SignalBus.SignalName.CardNodeReady);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        cooldownMask.Value = timer.TimeLeft / timer.WaitTime * (cooldownMask.MaxValue - cooldownMask.MinValue);
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
            {
                if (!costMask.Visible && cooldownMask.Value == 0)
                {
                    SignalBus.Instance.EmitSignal(SignalBus.SignalName.CardSelected, this);
                }
            }
        }
    }

    private void OnTimerTimeOut()
    {

    }

    private void OnGameTotalSunChanged(int amount)
    {
        costMask.Visible = amount < cost;
    }

    public void Consume()
    {
        SignalBus.Instance.EmitSignal(SignalBus.SignalName.CardCostSun, this);
    }

    public void Cooldown()
    {
        timer.Start();
    }
}
