using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArrow : MonoBehaviour
{
    public float pixelToMeterRatio = 1f;
    public Vector2 centerOffset;
    public Transform player;
    public RectTransform rectTransform;

    void Awake()
    {
        // centerPoint = GameObject.Find("World/CenterPoint").GetComponent<Transform>().position;
    }

    void Update()
    {
        rectTransform.anchoredPosition =
            new Vector2(-player.position.z * pixelToMeterRatio, player.position.x * pixelToMeterRatio) - centerOffset;
    }
}