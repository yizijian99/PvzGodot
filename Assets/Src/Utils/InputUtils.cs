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

    public static void SetCustomMouseCursor(Texture2D texture)
    {
        if (texture == null)
        {
            Input.SetDefaultCursorShape();
        }
        else
        {
            Input.SetCustomMouseCursor(texture, Input.CursorShape.Ibeam, texture.GetImage().GetSize() / 2);
        }
    }
}
