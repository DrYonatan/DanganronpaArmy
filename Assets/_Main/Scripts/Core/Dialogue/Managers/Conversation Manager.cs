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
        
        public bool isAuto = false;

        public bool isSingleTimeAuto = false;
        
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
            // StopPreviousText();

            process = dialogueSystem.StartCoroutine(RunNodeText(node));

            return process;
        }

        private void StopPreviousText()
        {
            if (!isRunning)
                return;
            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunNodeText(DialogueNode node)
        {
            VNTextData textData = node.textData as VNTextData;

            List<Command> beforeCommands = GetBeforeCommands(textData.commands);
            List<Command> parallelCommands = GetParallelCommands(textData.commands);
            List<Command> afterCommands = GetAfterCommands(textData.commands);
            
            yield return Line_RunCommands(beforeCommands);
            
            Line_RunCommandsAsync(parallelCommands);
            
            DialogueSystem.instance.ShowSpeakerName(node.character.displayName);
            yield return BuildDialogue(textData.text);

            yield return Line_RunCommands(afterCommands);

            if (isSingleTimeAuto)
            {
                isSingleTimeAuto = false;
            }
            else if (!isAuto)
            {
                yield return WaitForUserInput();
                SoundManager.instance.PlayTextBoxSound();
            }
        }

        List<Command> GetBeforeCommands(List<Command> commands)
        {
            return commands.FindAll((Command command) => command.executeTime == Command.ExecuteTime.Before);
        }
        
        List<Command> GetParallelCommands(List<Command> commands)
        {
            return commands.FindAll((Command command) => command.executeTime == Command.ExecuteTime.Parallel);
        }
        
        List<Command> GetAfterCommands(List<Command> commands)
        {
            return commands.FindAll((Command command) => command.executeTime == Command.ExecuteTime.After);
        }
        

        IEnumerator Line_RunCommands(List<Command> commands)
        {
            foreach (Command command in commands)
            {
                yield return command.Execute();
            }
        }

        void Line_RunCommandsAsync(List<Command> commands)
        {
            foreach (Command command in commands)
            {
                DialogueSystem.instance.StartCoroutineHelper(command.Execute());
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

        public IEnumerator WaitForUserInput()
        {
            while (!userPrompt)
                yield return null;
            userPrompt = false;
        }

        public void ClearTextBox()
        {
            architect.Clear();
        }
        
    }
}
