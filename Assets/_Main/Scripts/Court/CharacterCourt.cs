using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Idle = 0,
    Surprised = 1,
    Angry = 2
}

[CreateAssetMenu(menuName ="Data/CharacterCourt")]
public class CharacterCourt : ScriptableObject
{
    public string displayName;
    public List<Sprite> Sprites;
}
