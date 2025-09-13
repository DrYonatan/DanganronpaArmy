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
                yield return node.Play();
            }
        }

        public void HandleConversationEnd()
        {
            VNCharacterManager.instance.DestroyCharacters();
            WorldManager.instance.HandleConversationEnd();
        }

    }