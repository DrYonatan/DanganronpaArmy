using System;
using System.Collections.Generic;

[Serializable]
public class ConversationSettings
{
    public List<CharacterPositionMapping> characterPositions = new ();
}

[Serializable]
public class CharacterPositionMapping
{
    public Character character;
    public int position;
}