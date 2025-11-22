using System;
using System.Collections.Generic;

[Serializable]
public class ObjectDataEntry
{
    public string key;
    public ObjectData value;
}

[Serializable]
public class SaveData
{
    public int gameEventIndex;
    // public string currentRoom;
    // public string currentConversation;
    // public int currentConversationIndex;
    // public string currentMusic;
    // public List<ObjectDataEntry> objectDatas;
}
