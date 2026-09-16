using UnityEngine;
using TMPro;

public class WeedController : MonoBehaviour
{
    public Texture2D axeCursor;              // Axe cursor texture

   

    void Start()
    {
    }

    void OnMouseDown()
    {
        

            // Change cursor to axe
            Cursor.SetCursor(axeCursor, Vector2.zero, CursorMode.Auto);

           
    }

   
}
