using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotUtilities;

[Scene]
public partial class ProduceZombieAbility : Node
{
    [Export]
    private Node2D ground;

    [Export]
    private ZombieWaves waves;

    [Node("Timer")]
    private Timer timer;

    private Iterator<ZombieWave> iterator;

    private ZombieWave wave;

    private HashSet<IPosition> producePositionSet = new();

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        timer.Timeout += ProduceZombie;
        SignalBus.Instance.ZombieProducePositionReady += gdObj =>
        {
            if (gdObj is IPosition iPosition)
                AddProducePosition(iPosition);
        };

        iterator = waves.Iterator();
        NextWave();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public void ProduceZombie()
    {
        if (!(producePositionSet?.Any() ?? false))
            return;

        foreach (var i in Enumerable.Range(0, wave.zombieCount))
        {
            Random random = new Random();
            Vector2 producePosition = producePositionSet
                .OrderBy(x => random.Next())
                .Select(v => v.GetPosition())
                .FirstOrDefault();
            Zombie zombie = GlobalExport.Instance.zombie.Instantiate<Zombie>();
            zombie.GlobalPosition = producePosition;
            ground.AddChild(zombie);
        }

        NextWave();
    }

    public void AddProducePosition(IPosition position)
    {
        producePositionSet.Add(position);
    }

    private void NextWave()
    {
        if (iterator.HasNext())
        {
            wave = iterator.Next();
            timer.WaitTime = wave.delay;
            timer.Start();
        }
    }
}
