using UnityEngine;

public class Circle : MonoBehaviour
{
    public void DoubleVolute()
    {
        float scale = 1.5f * ((transform.localScale.x / 1.5f) + 1);
        transform.localScale = new(scale, scale, scale);
    }
}
