using Godot;
using GodotUtilities;

[Scene]
public partial class MainMenuManager : Node
{
    [Export]
    private BaseButton adventureBtn;

    [Export]
    private BaseButton quitBtn;

    [Export]
    private PackedScene adventureScene;

    public override void _Ready()
    {
        base._Ready();
        WireNodes();

        adventureBtn.Pressed += Adventure;
        quitBtn.Pressed += QuitGame;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    private void Adventure()
    {
        GetTree().ChangeSceneToPacked(adventureScene);
    }

    private void QuitGame()
    {
        GetTree().Quit();
    }
}
