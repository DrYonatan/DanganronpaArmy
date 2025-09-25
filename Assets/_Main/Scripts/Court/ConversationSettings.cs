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
    public CharacterCourt character;
    public int position;
}