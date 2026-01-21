using System;
using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using DIALOGUE;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    public Rect nodeRect;
    public Vector2 commandsScrollPosition;

    public string title;
    public DrawNode drawNode;

    public Character character;
    public string displayName;

    public int expressionIndex;

    [SerializeReference] public TextData textData;

    public DialogueNode(DrawNode drawNode)
    {
        this.drawNode = drawNode;
        this.InitializeTextData();
    }

    public DialogueNode(DrawNode drawNode, DialogueNode copy)
    {
        this.drawNode = drawNode;
        this.InitializeTextData();
        character = copy.character;
        displayName = copy.displayName;
        expressionIndex = copy.expressionIndex;
    }

    public void DrawNode(ConversationSettings settings, float windowWidth, float windowHeight)
    {
        if (drawNode != null)
        {
            drawNode.DrawWindow(this, settings, windowWidth, windowHeight);
        }
    }

    protected virtual void InitializeTextData()
    {
        textData = new VNTextData();
    }

    public virtual IEnumerator Play()
    {
        VNTextData data = textData as VNTextData;
        
        if(data == null)
            yield break;

        if (character == null)
            yield break;

        yield return DialogueSystem.instance.RunBeforeCommands(data.commands);
            
        if (VNNodePlayer.instance.currentConversation.settings != null && !character.notVisible)
        {
            CharacterPositionMapping info = VNNodePlayer.instance.currentConversation.settings.characterPositions.Find(characterInfo =>
                characterInfo.character == character);
            VNCharacterManager.instance.ShowOnlySpeaker(character, DialogueSystem.instance.GetIsSkip() ? 0 : 0.25f);
            VNCharacterManager.instance.SwitchEmotion(character, character.emotions[expressionIndex]);
            CameraManager.instance.MoveCamera((CameraLookDirection)info.position, DialogueSystem.instance.GetIsSkip() ? 0 : 0.4f);
        }
        
        yield return DialogueSystem.instance.Say(this);
    }
}