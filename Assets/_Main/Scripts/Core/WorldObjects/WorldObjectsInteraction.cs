using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class WorldObjectsInteraction : Interactable
{
    public bool isClicked = false;
    public TextAsset text1;
    public TextAsset text2;
    // Start is called before the first frame update
    void Start()
    {

    }
    
    public override void Interact()
    {
        if(!DialogueSystem.instance.isActive) 
        {
            SoundManager.instance.PlaySoundEffect("click");
            if(gameObject.transform.parent.name.Equals("Characters"))
            {
              CharacterClickEffects.instance.Interact(gameObject.transform);
            }
            StartCoroutine(MoveAndRotateCameraTo());
        }
        
    }

    IEnumerator MoveAndRotateCameraTo()
    {
        float duration = 0.5f;
        Vector3 targetPosition = Vector3.Lerp(Camera.main.transform.position, transform.position, 0.75f);
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up);

        yield return StartCoroutine(CameraManager.instance.RotateCameraTo(targetRotation, duration));
        StartCoroutine(CameraManager.instance.MoveCameraTo(targetPosition, duration));

        CameraManager.instance.setInitialPosition(targetPosition, targetRotation);
        if(WorldManager.instance.currentGameEvent != null)
            StartConversation();
        WorldManager.instance.currentGameEvent.UpdateEvent();
    }

    void StartConversation()
    {
        List<string> lines;
            if (!isClicked)
            {
                lines = FileManager.ReadTextAsset(text1);
            }
            else
            {
                lines = FileManager.ReadTextAsset(text2 ? text2 : text1);
            }
            DialogueSystem.instance.Say(lines);
            isClicked = true;

    }

}
