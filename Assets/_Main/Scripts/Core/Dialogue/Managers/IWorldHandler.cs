using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorldHandler
{
    public void HandleConversationEnd();
    public void PlayConversationNode(int index);
}
