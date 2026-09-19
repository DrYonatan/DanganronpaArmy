using DIALOGUE;
using UnityEngine;

public class PointAndClickMouseDetection : MonoBehaviour
{
    private void OnMouseOver()
    {
        if(WorldManager.instance.currentRoom is not PointAndClickRoom)
            return;
        
        if (!DialogueSystem.instance.isActive)
            WorldManager.instance.currentRoom.currentInteractable = gameObject.GetComponent<Interactable>();
    }

    private void OnMouseExit()
    {
        if(WorldManager.instance.currentRoom is not PointAndClickRoom)
            return;
        
        if(WorldManager.instance.currentRoom.currentInteractable != null)
           WorldManager.instance.currentRoom.currentInteractable.OnStopLooking();
        WorldManager.instance.currentRoom.currentInteractable = null;

    }
}