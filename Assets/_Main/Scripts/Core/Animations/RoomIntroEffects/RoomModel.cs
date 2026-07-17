using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomRendererLightmapData
{
    public string rendererPath;
    public int lightmapIndex;
    public Vector4 lightmapScaleOffset;
}

[Serializable]
public class RoomLightmapTextureData
{
    public Texture2D color;
    public Texture2D direction;
    public Texture2D shadowMask;
}

public class RoomModel : MonoBehaviour
{
    public List<Transform> talkPositions;
    public List<RoomIntroEffect> roomIntroEffects = new List<RoomIntroEffect>();
    public List<ConversationInteractable> interactables = new();

    [Header("Baked Lighting")] public List<RoomLightmapTextureData> lightmaps = new();
    public List<RoomRendererLightmapData> rendererLightmaps = new();
    public bool hasBakedLighting;

    public void ApplyLightmaps()
    {
        LightmapData[] data = new LightmapData[lightmaps.Count];

        for (int i = 0; i < lightmaps.Count; i++)
        {
            data[i] = new LightmapData
            {
                lightmapColor = lightmaps[i].color,
                lightmapDir = lightmaps[i].direction,
                shadowMask = lightmaps[i].shadowMask
            };
        }

        LightmapSettings.lightmaps = data;

        foreach (var rendererData in rendererLightmaps)
        {
            Transform target = transform.Find(rendererData.rendererPath);

            if (target == null)
                continue;

            MeshRenderer renderer = target.GetComponent<MeshRenderer>();

            if (renderer == null)
                continue;

            renderer.lightmapIndex = rendererData.lightmapIndex;
            renderer.lightmapScaleOffset = rendererData.lightmapScaleOffset;
        }
    }
    
    public void ClearLightmaps()
    {
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.lightmapIndex = -1;
            renderer.lightmapScaleOffset = new Vector4(1, 1, 0, 0);
        }

        LightmapSettings.lightmaps = new LightmapData[0];
    }


    public void PlayRoomIntroEffects()
    {
        foreach (RoomIntroEffect introEffect in roomIntroEffects)
        {
            StartCoroutine(introEffect.PlayEffect());
        }
    }
}