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

    public CharacterState expression;

    [SerializeReference] public TextData textData;

    public DialogueNode(DrawNode drawNode)
    {
        this.drawNode = drawNode;
        this.InitializeTextData();
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
        this.textData = new VNTextData();
    }

    public virtual IEnumerator Play()
    {
        CharacterPositionMapping info = VNNodePlayer.instance.currentConversation.settings.characterPositions.Find(characterInfo =>
            characterInfo.character == character);
        VNCharacterManager.instance.ShowOnlySpeaker(character);
        VNCharacterManager.instance.SwitchEmotion(character, expression);
        CameraManager.instance.MoveCamera((CameraLookDirection)info.position, 0.4f);
        yield return DialogueSystem.instance.Say(this);
    }
}