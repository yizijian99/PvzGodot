using Godot;

public class InputUtils
{
    private InputUtils()
    {

    }

    public static bool MouseLeftButtonPressed(InputEvent @event)
    {
        return @event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left;
    }
}
