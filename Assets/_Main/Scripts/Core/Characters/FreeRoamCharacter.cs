using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class FreeRoamCharacter : FreeRoamInteractable
{
    public TextAsset textToSay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    public override void Interact()
    {
        CharacterClickEffects.instance.Interact(gameObject.transform);
        DialogueSystem.instance.Say(FileManager.ReadTextAsset(textToSay));
        SoundManager.instance.PlaySoundEffect("click");
    }
}
