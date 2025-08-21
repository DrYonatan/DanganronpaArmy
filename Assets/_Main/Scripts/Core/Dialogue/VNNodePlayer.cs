using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using DIALOGUE;
using UnityEngine;
    public class VNNodePlayer : MonoBehaviour
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
            StartCoroutine(RunConversation(segment));
        }

        IEnumerator RunConversation(VNConversationSegment segment)
        {
            yield return RunNodes(segment.nodes);
            HandleConversationEnd();
        }

        IEnumerator RunNodes(List<DialogueNode> nodes)
        {
            foreach (DialogueNode node in nodes)
            {
                yield return PlayConversationNode(node);
            }
        }
        

        public IEnumerator PlayConversationNode(DialogueNode node)
        {
            CharacterCourt speaker = node.character;
            VNCharacterInfo info =
                currentConversation.CharacterInfos.Find(characterInfo => characterInfo.Character == speaker);
            VNCharacterManager.instance.ShowOnlySpeaker(info);
            VNCharacterManager.instance.SwitchEmotion(info.Character, node.expression);
            CameraManager.instance.MoveCamera(info.LookDirection, 0.4f);
            yield return DialogueSystem.instance.Say(node);
        }

        public void HandleConversationEnd()
        {
            VNCharacterManager.instance.DestroyCharacters();
            WorldManager.instance.HandleConversationEnd();
        }

    }