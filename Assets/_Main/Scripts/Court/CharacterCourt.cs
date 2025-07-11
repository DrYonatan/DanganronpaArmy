using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Idle = 0,
    Surprised = 1,
    angry = 2
}

[CreateAssetMenu(menuName ="Data/CharacterCourt")]
public class CharacterCourt : ScriptableObject
{
    public string name;
    public List<Sprite> Sprites;
}
