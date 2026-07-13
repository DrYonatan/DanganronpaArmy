using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using DIALOGUE;
using UnityEngine;

public class VNNodePlayer : MonoBehaviour
{
    public static VNNodePlayer instance { get; private set; }
    public VNConversationSegment currentConversation;
    public int lineIndex;
    private VNConversationSegment queuedConversation;
    private void Awake()
    {
        instance = this;
    }

    public void StartConversation(VNConversationSegment segment)
    {
        StartCoroutine(StartConversationPipeline(segment));
    }
    public IEnumerator StartConversationPipeline(VNConversationSegment segment)
    {
        currentConversation = segment;
        VNCharacterManager.instance.characterLayer.anchoredPosition = Vector2.zero;
        foreach (CharacterPositionMapping characterInfo in segment.settings.characterPositions)
        {
            VNCharacterManager.instance.CreateCharacter(characterInfo);
        }

        yield return RunConversation(segment);
    }

    public void AddToQueue(VNConversationSegment segment)
    {
        queuedConversation = segment;
    }

    IEnumerator RunConversation(VNConversationSegment segment)
    {
        yield return RunNodes(segment.nodes);
        if (queuedConversation != null)
        {
            lineIndex = 0;
            VNCharacterManager.instance.DestroyCharacters();
            
            var next = queuedConversation;
            queuedConversation = null;

            StartConversation(next);
        }
        else
           HandleConversationEnd();
    }

    private IEnumerator RunNodes(List<DialogueNode> nodes)
    {
        VNCharacterManager.instance.HideAllCharacters();
        for (int i = lineIndex; i < nodes.Count; i++)
        {
            yield return RunNode(nodes[i]);
            DialogueSystem.instance.ClearTextBox();
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