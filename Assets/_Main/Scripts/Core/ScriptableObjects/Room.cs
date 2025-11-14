using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Room : ScriptableObject
{
    public GameObject prefab;

    public Interactable currentInteractable;

    public bool isInside = false;

    public abstract void MovementControl();

    public virtual IEnumerator OnLoad()
    {
        CameraManager.instance?.ChangeCameraBackground(isInside);
        return null;
    }

    public abstract void OnConversationEnd();
}