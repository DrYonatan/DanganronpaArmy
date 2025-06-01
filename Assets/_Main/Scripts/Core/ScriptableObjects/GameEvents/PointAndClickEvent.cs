using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

[CreateAssetMenu(menuName = "Game Events/Point And Click Event")]
public class PointAndClickEvent : GameEvent
{
    public GameObject characterPrefab;
    public GameObject interactableObjectsPrefab;
    public static string characterPath = $"World/World Objects/Characters";
    public static string objectsPath = $"World/World Objects/Objects";
    public bool isExitable = false;

    public override void UpdateEvent()
    {
        isFinished = true;
        Transform characters = GameObject.Find(characterPath)?.transform;

        // checks all the characters are clicked
        if (characters != null)
        {
            foreach (Transform character in characters)
            {
                if (!character.GetComponent<WorldObjectsInteraction>().isClicked)
                    isFinished = false;
            }
        }

        Transform objects = GameObject.Find(objectsPath)?.transform;

        // checks all the objects are clicked
        if (objects != null)
        {
            foreach (Transform interactableObject in objects)
            {
                if (!interactableObject.GetComponent<WorldObjectsInteraction>().isClicked)
                    isFinished = false;
            }
        }

        if (objects == null &&
            characters == null) // if none of the interactables loaded it means the event just started
            isFinished = false;
    }

    public override void CheckIfFinished()
    {
        if (isFinished)
            OnFinish();
        else
            DialogueSystem.instance.SetIsActive(false);
    }


    public override void PlayEvent()
    {
        GameObject characters = GameObject.Find(characterPath);
        GameObject objects = GameObject.Find(objectsPath);
        ((PointAndClickRoom)(WorldManager.instance.currentRoom)).ResetRotations();

        if (characterPrefab != null)
        {
            if (characters == null)
            {
                WorldManager.instance.CreateCharacters(characterPrefab);
            }
            else
            {
                CharacterClickEffects.instance.MakeCharactersReappear(characters);
            }
        }


        if (objects == null && interactableObjectsPrefab != null)
        {
            WorldManager.instance.CreateObjects(interactableObjectsPrefab);
        }
    }

    public override void OnFinish()
    {
        if (!isExitable)
        {
            GameObject characters = GameObject.Find(characterPath);
            GameObject objects = GameObject.Find(objectsPath);
            Destroy(objects);
            Destroy(characters);
            CameraManager.instance.MoveCameraTo(GameObject.Find("World/CameraStartPos").transform);
        }

        base.OnFinish();
    }
}