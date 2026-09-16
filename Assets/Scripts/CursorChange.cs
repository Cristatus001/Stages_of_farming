using UnityEngine;

public class CursorChange : MonoBehaviour
{
    public Texture2D cursorImage;

    private void Start()
    {
        Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.Auto);
    }
}
