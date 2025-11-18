using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

[CreateAssetMenu(menuName = "Game Events/Point And Click Event")]
public class PointAndClickEvent : GameEvent
{
    public bool isExitable = false;

    private bool AreAllClicked(GameObject objects)
    {
        bool finished = true;

        if (objects != null)
        {
            foreach (Transform interactableObject in objects.transform)
            {
                if (!interactableObject.GetComponent<ConversationInteractable>().isClicked)
                    finished = false;
            }
        }

        return finished;
    }

    public override void CheckIfFinished()
    {
        bool allCharactersClicked = AreAllClicked(WorldManager.instance.charactersObject);

        bool allObjectsClicked = AreAllClicked(WorldManager.instance.objectsObject);

        isFinished = allCharactersClicked && allObjectsClicked;

        if (isFinished)
            OnFinish();
    }


    public override void OnStart()
    {
        
    }

    protected override void OnFinish()
    {
        if (!isExitable)
        {
            Destroy(WorldManager.instance.charactersObject);
            Destroy(WorldManager.instance.objectsObject);
            CameraManager.instance.MoveCameraTo(GameObject.Find("World/CameraStartPos").transform);
        }

        base.OnFinish();
    }
}