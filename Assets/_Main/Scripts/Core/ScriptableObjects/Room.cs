using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Room : ScriptableObject
{
    public GameObject prefab;

    public Interactable currentInteractable;

    public abstract void MovementControl();

    public abstract IEnumerator OnLoad();
   
}
