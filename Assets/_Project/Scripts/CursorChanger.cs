using UnityEngine;

public class CursorChanger
{
    private Vector2 hotSpot = Vector2.zero;

    public void ChangeCursorIcon(Texture2D icon)
    {
        Cursor.SetCursor(icon, hotSpot, CursorMode.Auto);
    }

}