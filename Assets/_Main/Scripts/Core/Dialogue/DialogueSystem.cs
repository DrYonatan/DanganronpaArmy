using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueSystem : MonoBehaviour
    {
        public bool isActive;

        public DialogueContainer dialogueContainer = new DialogueContainer();
        private ConversationManager conversationManager;
        private TextArchitect architect;
        
        public static DialogueSystem instance { get; private set; }

        public delegate void DialogueSystemEvent();

        public event DialogueSystemEvent onUserPrompt_Next;

        public TextBoxAnimations dialogueBoxAnimator;

        private void Awake()
        {
            isActive = false;
            if (instance == null)
            {
                instance = this;
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
                ClearSpeakerName();
        }

        public void ClearSpeakerName() => dialogueContainer.nameContainer.Clear();
        
        public Coroutine Say(DialogueNode node)
        {
            if(!isActive)
            SetIsActive(true);
            return conversationManager.PlayNodeText(node);
        }

        public void SetIsActive(bool isActive) // Not to be cofnsued with Unity's GameObject.SetActive() 
        {
            this.isActive = isActive;

            if (isActive)
            {
                VirutalCameraManager.instance?.DisableVirtualCamera();
                dialogueBoxAnimator.TextBoxAppear();
                CursorManager.instance.Hide();
            }
            else
            {
                dialogueBoxAnimator.TextBoxDisappear();
                CursorManager.instance.Show();
            }
        }

        public void ClearTextBox()
        {
            ClearSpeakerName();
            conversationManager.ClearTextBox();
        }
    }
}