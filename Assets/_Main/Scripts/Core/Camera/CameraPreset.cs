using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Camera Preset")]
public class CameraPreset : ScriptableObject
{
    [Header("Offsets")]
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public float fovOffset;

    [Header("Effects")]
    public List<CameraEffect> cameraEffects = new List<CameraEffect>();
}
