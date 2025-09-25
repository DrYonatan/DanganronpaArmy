using System.Collections;
using System.Collections.Generic;
using COMMANDS;
using UnityEngine;

namespace DIALOGUE
{
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;

        private Coroutine process = null;
        public bool isRunning => process != null;

        private TextArchitect architect = null;

        private bool userPrompt = false;
        
        public ConversationManager(TextArchitect architect)
        {
            this.architect = architect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }

        public void SetArchitect(TextArchitect architect)
        {
            this.architect = architect;
        }

        private void OnUserPrompt_Next()
        {
            userPrompt = true;
        }

        public Coroutine PlayNodeText(DialogueNode node)
        {
            StopPreviousText();

            process = dialogueSystem.StartCoroutine(RunNodeText(node));

            return process;
        }

        public void StopPreviousText()
        {
            if (!isRunning)
                return;
            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunNodeText(DialogueNode node)
        {
            VNTextData textData = node.textData as VNTextData;
            DialogueSystem.instance.ShowSpeakerName(node.character.displayName);
            yield return Line_RunCommands(textData.commands);
            yield return BuildDialogue(textData.text);
            yield return WaitForUserInput();
            SoundManager.instance.PlayTextBoxSound();
        }
        

        IEnumerator Line_RunCommands(List<Command> commands)
        {
            foreach (Command command in commands)
            {
                command.Execute();
                yield return null;
            }
            
        }
        

        IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            //Build dialogue
            if (!append)
                architect.Build(dialogue);
            else
                architect.Append(dialogue);

            //Wait for the dialogue to complete
            while (architect.isBuilding)
            {
                if (userPrompt)
                {
                    architect.ForceComplete();

                    userPrompt = false;
                }
                yield return null;
            }

            

        }

        IEnumerator WaitForUserInput()
        {
            while (!userPrompt)
                yield return null;
            userPrompt = false;
        }

        public void ClearTextBox()
        {
            architect.Clear();
        }

        public IEnumerator HandleSelection()
        {
            yield return WaitForUserInput();
        }
    }
}
