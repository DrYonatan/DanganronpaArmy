using UnityEngine;

public class MapArrow : MonoBehaviour
{
    public float pixelToMeterRatio = 1f;
    public Vector2 centerOffset;
    public Transform player;
    public RectTransform rectTransform;

    void Update()
    {
        rectTransform.anchoredPosition =
            new Vector2(-player.position.z * pixelToMeterRatio, player.position.x * pixelToMeterRatio) - centerOffset;
        rectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90 - player.eulerAngles.y));
    }
}