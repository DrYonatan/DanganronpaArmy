using System.Collections.Generic;
using CHARACTERS;
using DIALOGUE;
using UnityEngine;
    public class VNNodePlayer : MonoBehaviour, IConversationNodePlayer
    {
        public static VNNodePlayer instance { get; private set; }
        public VNConversationSegment currentConversation;
        private void Awake()
        {
            instance = this;
        }
        
        public void StartConversation(VNConversationSegment segment)
        {
            this.currentConversation = segment;
            foreach (VNCharacterInfo characterInfo in segment.CharacterInfos)
            {
                VNCharacterManager.instance.CreateCharacter(characterInfo);
            }
            DialogueSystem.instance.Say(segment.nodes);
        }

        public void PlayConversationNode(int index)
        {
            CharacterCourt speaker = currentConversation.nodes[index].character;
            VNCharacterInfo info =
                currentConversation.CharacterInfos.Find(characterInfo => characterInfo.Character == speaker);
            VNCharacterManager.instance.ShowOnlySpeaker(info);
            if(index > 0 && currentConversation.nodes[index].expression != currentConversation.nodes[index-1].expression)
            VNCharacterManager.instance.SwitchEmotion(info.Character, currentConversation.nodes[index].expression);
            CameraManager.instance.MoveCamera(info.LookDirection, 0.4f);
        }

        public void HandleConversationEnd()
        {
            VNCharacterManager.instance.DestroyCharacters();
            WorldManager.instance.HandleConversationEnd();
        }

    }