using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIALOGUE
{
    public class DialogueSystem : MonoBehaviour
    {
        public bool isActive;

        private ConversationManager conversationManager;
        private TextArchitect architect;

        public TextBoxAnimator defaultDialogueBoxAnimator;
        public static DialogueSystem instance { get; private set; }
        
        public delegate void DialogueSystemEvent();

        public event DialogueSystemEvent onUserPrompt_Next;

        public BasicTextBoxAnimator dialogueBoxAnimator;
        public OptionSelectionManager optionSelectionManager;
        public Button inputButton;

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

            architect = new TextArchitect(dialogueBoxAnimator.dialogueContainer.dialogueText);
            conversationManager = new ConversationManager(architect);
        }

        public void UseInitialDialogueContainer()
        {
            SetTextBox(defaultDialogueBoxAnimator);
        }

        public void OnUserPrompt_Next()
        {
            onUserPrompt_Next?.Invoke();
        }

        public void ShowSpeakerName(DialogueNode node)
        {
            if (node.displayName.ToLower() != "")
                dialogueBoxAnimator.dialogueContainer.nameContainer.Show(node.displayName);
            
            else if (node.character != null && !node.character.noNameTag)
            {
                if(!dialogueBoxAnimator.namePlateVisible)
                    ShowNamePlate();
                dialogueBoxAnimator.dialogueContainer.nameContainer.Show(node.character.displayName);
            }
            else
            {
                ClearSpeakerName();
                HideNamePlate();
            }
        }

        public void HideNamePlate()
        {
            GameStateManager.instance.uiState.namePlateVisible = false;
            dialogueBoxAnimator.HideNamePlate();
        }

        public void ShowNamePlate()
        {
            GameStateManager.instance.uiState.namePlateVisible = true;
            dialogueBoxAnimator.ShowNamePlate();
        }
        

        void ClearSpeakerName() => dialogueBoxAnimator.dialogueContainer.nameContainer.Clear();

        public Coroutine Say(DialogueNode node)
        {
            if (!isActive)
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
                inputButton.gameObject.SetActive(true);
            }
            else
            {
                dialogueBoxAnimator.TextBoxDisappear();
                inputButton.gameObject.SetActive(false);
            }
        }

        public void ClearTextBox()
        {
            ClearSpeakerName();
            conversationManager.ClearTextBox();
        }

        public void StartCoroutineHelper(IEnumerator enumerator)
        {
            StartCoroutine(enumerator);
        }

        public void TextBoxAppear()
        {
            dialogueBoxAnimator.TextBoxAppear();
        }

        public void TextBoxDisappear()
        {
            dialogueBoxAnimator.TextBoxDisappear();
        }

        public void SetTextBox(BasicTextBoxAnimator textBox)
        {
            dialogueBoxAnimator = textBox;
            architect = new TextArchitect(dialogueBoxAnimator.dialogueContainer.dialogueText);
            conversationManager.SetArchitect(architect);
            dialogueBoxAnimator.Initialize();
        }

        public IEnumerator HandleSelection<T>(List<Option<T>> options, Action<Option<T>> onSelect)
            where T : DialogueNode
        {
            optionSelectionManager.OpenMenu(options);
            yield return conversationManager.WaitForUserInput();
            optionSelectionManager.ClickSelectedOption();
            yield return new WaitForSeconds(0.2f);
            optionSelectionManager.CloseMenu();
            onSelect(options[optionSelectionManager.selectedIndex]);
        }

        public void SetAuto(bool isAuto)
        {
            conversationManager.isAuto = isAuto;
        }

        public void SetSkip(bool isSkip)
        {
            conversationManager.isSkip = isSkip;
        }

        public bool GetIsSkip()
        {
            return conversationManager.isSkip;
        }


        public void TurnOnSingleTimeAuto()
        {
            conversationManager.isSingleTimeAuto = true;
        }

        public IEnumerator RunBeforeCommands(List<Command> commands)
        {
            yield return conversationManager.Line_RunCommands(conversationManager.GetBeforeCommands(commands));
        }
    }
}