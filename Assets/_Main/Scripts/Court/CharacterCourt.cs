using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CharacterState
{
    public string name;
    public Sprite sprite;
}

[CreateAssetMenu(menuName ="Data/CharacterCourt")]
public class CharacterCourt : ScriptableObject
{
    public string displayName;
    public GameObject vnObjectPrefab;
    public List<CharacterState> emotions;
    public Sprite faceSprite;
    
    public CharacterState FindStateByName(string stateName)
    {
        if (emotions == null)
            return null;

        return emotions.FirstOrDefault(e => e.name == stateName);
    }
}
