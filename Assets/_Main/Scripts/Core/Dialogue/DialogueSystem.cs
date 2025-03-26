using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace DIALOGUE
{


    public class DialogueSystem : MonoBehaviour
    {

        public bool isActive;
        [SerializeField] private DialogueSystemConfigurationSO _config;
        public DialogueSystemConfigurationSO config => _config;

        public DialogueContainer dialogueContainer = new DialogueContainer();
        private ConversationManager conversationManager;
        private TextArchitect architect;
        public bool isRunningConversation => conversationManager.isRunning;


        public static DialogueSystem instance { get; private set; }

        public delegate void DialogueSystemEvent();
        public event DialogueSystemEvent onUserPrompt_Next;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                isActive = false;
                Initialize();
            }

            else
                DestroyImmediate(gameObject);
        }

        bool _initialized = false;
        private void Initialize()
        {
            if (_initialized)
                return;

            architect = new TextArchitect(dialogueContainer.dialogueText);
            conversationManager = new ConversationManager(architect);
        }

        public void OnUserPrompt_Next()
        {
            onUserPrompt_Next?.Invoke();
        }

        public void ShowSpeakerName(string speakerName = "")
        {
            if (speakerName.ToLower() != "narrator")
                dialogueContainer.nameContainer.Show(speakerName);
            else
                HideSpeakerName();
        }
        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();
        public Coroutine Say(string speaker, string dialogue)
        {
           List<string> conversation = new List<string>() { $"{speaker} \"{dialogue}\"" };
            return Say(conversation);
        }

        public Coroutine Say(List<string> conversation)
        {
           isActive = true;
           GameObject dialogueBox = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/4 - Dialogue");
           dialogueBox.SetActive(true);
           dialogueBox.GetComponent<CanvasGroup>().alpha = 1;
           return conversationManager.StartConversation(conversation);
        }
}

}
