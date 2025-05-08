using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterFreeTimeEvents
{
    public string characterName;
    public List<TextAsset> events;
}

[CreateAssetMenu(fileName="Character Free TIme Events Asset", menuName="Game Events/Free Time Event Asset")]
public class CharactersFreeTimeEventsSO : ScriptableObject
{
    public List<CharacterFreeTimeEvents> charactersEvents;
}