using Godot;
using GodotUtilities;

namespace Pvz.Assets.Scr.Zombies;

[Scene]
public partial class Zombie : CharacterBody2D
{
    [Export]
    private AnimationPlayer animationPlayer;
    
    [Export]
    private Vector2 Direction { get; set; } = Vector2.Left;

    [Export]
    private int Speed { get; set; } = 32;

    [Export]
    private float SpeedScale { get; set; } = 1f;

    [Export]
    private ZombieState state;
    
    public ZombieState State
    {
        get => state;
        set
        {
            state = value;
            if (animationPlayer != null)
            {
                animationPlayer.Play(state.ToString());
            }
        }
    }

    [Export]
    private float stepSpeedScale = 1f;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        State = state;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Velocity = Direction * Speed * stepSpeedScale * SpeedScale;
        MoveAndSlide();
    }

    public enum ZombieState
    {
        Move1,
        Move2,
    }
}