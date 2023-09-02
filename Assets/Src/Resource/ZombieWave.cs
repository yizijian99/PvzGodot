using Godot;

[GlobalClass]
public partial class ZombieWave : Resource
{
    [Export]
    public float delay;

    [Export]
    public int zombieCount;
}
