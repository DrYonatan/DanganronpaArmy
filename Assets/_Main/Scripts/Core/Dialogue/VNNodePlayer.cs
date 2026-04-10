using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using UnityEngine;

public class VNNodePlayer : MonoBehaviour
{
    public static VNNodePlayer instance { get; private set; }
    public VNConversationSegment currentConversation;
    public int lineIndex;

    private void Awake()
    {
        instance = this;
    }

    public void StartConversation(VNConversationSegment segment)
    {
        currentConversation = segment;
        VNCharacterManager.instance.characterLayer.anchoredPosition = Vector2.zero;
        foreach (CharacterPositionMapping characterInfo in segment.settings.characterPositions)
        {
            VNCharacterManager.instance.CreateCharacter(characterInfo);
        }

        StartCoroutine(RunConversation(segment));
    }

    IEnumerator RunConversation(VNConversationSegment segment)
    {
        yield return RunNodes(segment.nodes);
        HandleConversationEnd();
    }

    public IEnumerator RunNodes(List<DialogueNode> nodes)
    {
        VNCharacterManager.instance.HideAllCharacters();
        for (int i = lineIndex; i < nodes.Count; i++)
        {
            yield return RunNode(nodes[i]);
            lineIndex++;
        }
    }

    public IEnumerator RunNode(DialogueNode node)
    {
        yield return node.Play();
        yield return new WaitUntil(() => CameraManager.instance.conversationFinishedMoving);
    }

    private void HandleConversationEnd()
    {
        VNCharacterManager.instance.DestroyCharacters();
        currentConversation = null;
        lineIndex = 0;
        WorldManager.instance.HandleConversationEnd();
    }
}