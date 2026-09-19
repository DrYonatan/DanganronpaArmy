using UnityEngine;

public abstract class CameraEffect : ScriptableObject
{
    public float timeLimit = 40f;
    public abstract void Apply(CameraEffectController effectController);
}
