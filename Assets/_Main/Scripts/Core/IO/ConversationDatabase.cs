using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class ConversationDatabase : ScriptableObject
{
    public List<VNConversationSegment> conversations;

    private Dictionary<string, VNConversationSegment> lookup;

    public VNConversationSegment Get(string guid)
    {
        if (lookup == null)
            lookup = conversations.ToDictionary(c => c.guid, c => c);

        return lookup[guid];
    }
}