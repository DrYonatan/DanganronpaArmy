using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using System;

public class CharacterStand : MonoBehaviour
{

    public CharacterCourt character;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer silhouetteRenderer;
    public Transform heightPivot;
    // Start is called before the first frame update
    void Start()
    {
        SetSprite(character.FindStateByName("default"));
    }

  

    internal void SetSprite(CharacterState state)
    {
        if (state != null)
        {
            spriteRenderer.sprite = state.sprite;
            silhouetteRenderer.sprite = state.sprite;
        }
    }
}
