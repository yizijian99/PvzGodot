using Godot;
using GodotUtilities;

[Scene]
public partial class Card : TextureRect
{
    [Node("Timer")]
    private Timer timer;

    [Node("CostMask")]
    private ColorRect costMask;

    [Node("CoolDownMask")]
    private TextureProgressBar coolDownMask;

    [Export]
    private string cardName;

    [Export]
    public Texture2D preview { get; private set; }

    [Export]
    public PackedScene plantScene { get; private set; }

    [Export]
    public int cost { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        GuiInput += OnGuiInput;
        SignalBus.Instance.Game_TotalSunsChanged += OnGameTotalSunChanged;

        SignalBus.Instance.EmitSignal(SignalBus.SignalName.Card_NodeReady);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        coolDownMask.Value = timer.TimeLeft / timer.WaitTime * (coolDownMask.MaxValue - coolDownMask.MinValue);
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (InputUtils.MouseLeftButtonPressed(@event) && !costMask.Visible && coolDownMask.Value == 0)
        {
            SignalBus.Instance.EmitSignal(SignalBus.SignalName.Card_BeClicked, this);
        }
    }

    private void OnGameTotalSunChanged(int oldValue, int newValue)
    {
        costMask.Visible = newValue < cost;
    }

    public void CoolDown()
    {
        timer.Start();
    }
}
