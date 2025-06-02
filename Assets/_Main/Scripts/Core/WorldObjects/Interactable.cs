using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    

    public virtual void Interact()
    {
        SoundManager.instance.PlaySoundEffect("click");
    }

    
}