using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

[CreateAssetMenu(menuName = "Game Events/Point And Click Event")]
public class PointAndClickEvent : GameEvent
{
    public GameObject characterPrefab;
    public GameObject interactableObjectsPrefab;
    public GameObject characters;
    public GameObject objects;
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
        bool allCharactersClicked = AreAllClicked(characters);

        bool allObjectsClicked = AreAllClicked(objects);

        isFinished = allCharactersClicked && allObjectsClicked && objects != null &&
                     characters != null; // if none of the interactables loaded it means the event just started

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
            Destroy(objects);
            Destroy(characters);
            CameraManager.instance.MoveCameraTo(GameObject.Find("World/CameraStartPos").transform);
        }

        base.OnFinish();
    }
}