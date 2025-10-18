using UnityEngine;
using UnityEngine.UI;

public class MapContainer : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Image map;
    
    public static MapContainer instance {get; private set;}

    void Awake()
    {
        instance = this;
    }

    public void SetMap(Sprite map)
    {
        this.map.sprite = map;
    }

    public void HandleMapVisibility()
    {
        canvasGroup.alpha = Input.GetKey(KeyCode.Tab) ? 0.8f : 0f;
    }

    public void HideMap()
    {
        canvasGroup.alpha = 0f;
    }
}