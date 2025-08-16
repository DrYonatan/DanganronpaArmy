using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using System;

public class CharacterStand : MonoBehaviour
{

    public CharacterCourt character;
    public CharacterState state;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer silhouetteRenderer;
    public Transform heightPivot;
    // Start is called before the first frame update
    void Start()
    {
        SetSprite();
    }

  

    internal void SetSprite()
    {
        
        int stateIndex = (int)state;
        if (character.Sprites.Count < stateIndex)
        {
            spriteRenderer.sprite = character.Sprites[0];
            silhouetteRenderer.sprite = character.Sprites[0];
            return;
        }
        if(character.Sprites[stateIndex] == null)
        {
            spriteRenderer.sprite = character.Sprites[0];
            silhouetteRenderer.sprite = character.Sprites[0];
            return;
        }
        spriteRenderer.sprite = character.Sprites[stateIndex];
        silhouetteRenderer.sprite = character.Sprites[stateIndex];
    }
}
