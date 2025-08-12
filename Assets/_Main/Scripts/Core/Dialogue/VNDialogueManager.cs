using DIALOGUE;
using UnityEngine;
    public class VNDialogueManager : MonoBehaviour, IConversationNodePlayer
    {
        public static VNDialogueManager instance { get; private set; }
        public VNConversationSegment currentConversation;
        private void Awake()
        {
            instance = this;
        }
        
        public void StartConversation(VNConversationSegment segment)
        {
            this.currentConversation = segment;
            DialogueSystem.instance.Say(segment.nodes);
        }

        public void PlayConversationNode(int index)
        {
            CharacterCourt speaker = currentConversation.nodes[index].character;
            VNCharacterInfo info =
                currentConversation.CharacterInfos.Find(characterInfo => characterInfo.Character == speaker);
            CameraManager.instance.MoveCamera(info.LookDirection, 0.4f);
        }

        public void HandleConversationEnd()
        {
            WorldManager.instance.HandleConversationEnd();
        }
    }