using UnityEngine;

public class MapContainer : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            canvasGroup.alpha = 0.8f;
        }
        else
        {
            canvasGroup.alpha = 0f;
        }
    }
}
