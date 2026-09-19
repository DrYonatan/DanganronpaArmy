using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Character Free Time Events Asset", menuName="Game Events/Free Time Event Asset")]
public class CharactersFreeTimeEventsSO : ScriptableObject
{
    public List<CharacterFreeTimeEvents> charactersEvents;
}

[Serializable]
public class CharacterFreeTimeEvents
{
    public Character character;
    public List<VNConversationSegment> events;
}