using UnityEngine;

public class ClickCounter : MonoBehaviour
{
    public int clickCount = 0;

    public int IncrementClickCount()
    {
        clickCount++;
        return clickCount;
    }
}
