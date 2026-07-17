using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(RoomModel))]
public class RoomModelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RoomModel room = (RoomModel)target;

        GUILayout.Space(20);

        if (GUILayout.Button("Save Room Lightmaps"))
        {
            SaveLightmaps(room);
        }
    }


    private void SaveLightmaps(RoomModel room)
    {
        if (LightmapSettings.lightmaps == null ||
            LightmapSettings.lightmaps.Length == 0)
        {
            Debug.LogError("No baked lightmaps found.");
            return;
        }

        room.hasBakedLighting = true;


        string folder =
            $"Assets/RoomLightmaps/{room.name}";

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);


        room.lightmaps.Clear();
        room.rendererLightmaps.Clear();


        // Save textures
        foreach (LightmapData lightmap in LightmapSettings.lightmaps)
        {
            RoomLightmapTextureData data =
                new RoomLightmapTextureData();

            data.color = CopyTexture(
                lightmap.lightmapColor,
                folder);

            data.direction = CopyTexture(
                lightmap.lightmapDir,
                folder);

            data.shadowMask = CopyTexture(
                lightmap.shadowMask,
                folder);

            room.lightmaps.Add(data);
        }


        // Save renderer info
        MeshRenderer[] renderers =
            room.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer.lightmapIndex < 0)
                continue;


            room.rendererLightmaps.Add(
                new RoomRendererLightmapData
                {
                    rendererPath =
                        GetPath(renderer.transform, room.transform),

                    lightmapIndex =
                        renderer.lightmapIndex,

                    lightmapScaleOffset =
                        renderer.lightmapScaleOffset
                });
        }
        

        EditorUtility.SetDirty(room);

        Debug.Log(
            $"Saved {room.lightmaps.Count} lightmaps and {room.rendererLightmaps.Count} renderers."
        );
    }


    private Texture2D CopyTexture(Texture2D source, string folder)
    {
        if (source == null)
            return null;


        string path =
            AssetDatabase.GetAssetPath(source);

        string fileName =
            Path.GetFileName(path);

        string destination =
            $"{folder}/{fileName}";


        if (!File.Exists(destination))
        {
            File.Copy(path, destination);
            AssetDatabase.Refresh();
        }


        return AssetDatabase.LoadAssetAtPath<Texture2D>(
            destination);
    }


    private string GetPath(
        Transform current,
        Transform root)
    {
        if (current == root)
            return "";

        return GetPath(current.parent, root)
               + "/" + current.name;
    }
}