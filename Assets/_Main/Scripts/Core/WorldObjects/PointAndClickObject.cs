using DIALOGUE;
using UnityEngine;

public class PointAndClickObject : MonoBehaviour
{
    private void OnMouseOver()
    {
        if (!DialogueSystem.instance.isActive)
            WorldManager.instance.currentRoom.currentInteractable = gameObject.GetComponent<Interactable>();
    }

    private void OnMouseExit()
    {
        if(WorldManager.instance.currentRoom.currentInteractable != null)
           WorldManager.instance.currentRoom.currentInteractable.OnStopLooking();
        WorldManager.instance.currentRoom.currentInteractable = null;

    }
}