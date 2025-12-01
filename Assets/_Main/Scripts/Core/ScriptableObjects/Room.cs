using System.Collections;
using UnityEngine;

public abstract class Room : ScriptableObject
{
    public RoomModel prefab;

    public Interactable currentInteractable;

    public bool isInside = false;

    public abstract void MovementControl();

    public virtual IEnumerator OnLoad()
    {
        CameraManager.instance?.ChangeCameraBackground(isInside);
        return null;
    }

    public virtual IEnumerator AppearAnimation()
    {
        yield return null;
    }

    public abstract void OnConversationEnd();
}