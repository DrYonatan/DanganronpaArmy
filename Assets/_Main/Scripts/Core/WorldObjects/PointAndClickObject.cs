using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class PointAndClickObject : MonoBehaviour
{
    private void OnMouseDown()
    {
        gameObject.GetComponent<WorldObjectsInteraction>().Interact();
    }

    private void OnMouseOver()
    {
        if(!DialogueSystem.instance.isActive)
        WorldManager.instance.currentRoom.currentInteractable = gameObject.GetComponent<Interactable>();
    }

    private void OnMouseExit()
    {
        WorldManager.instance.currentRoom.currentInteractable = null;
    }
}
