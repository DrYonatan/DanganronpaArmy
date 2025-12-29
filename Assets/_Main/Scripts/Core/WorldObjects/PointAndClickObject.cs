using DIALOGUE;
using UnityEngine;

public class PointAndClickObject : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (!DialogueSystem.instance.isActive)
        {
            gameObject.GetComponent<Interactable>().Interact();
            VirutalCameraManager.instance.virtualCamera.gameObject.SetActive(false);
        }
    }

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