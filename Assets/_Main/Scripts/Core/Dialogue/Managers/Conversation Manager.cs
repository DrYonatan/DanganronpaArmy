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
        public bool isSkip = false;

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

            if (textData == null)
                yield break;

            List<Command> parallelCommands = GetParallelCommands(textData.commands);
            List<Command> afterCommands = GetAfterCommands(textData.commands);
            
            Line_RunCommandsAsync(parallelCommands);

            DialogueSystem.instance.ShowSpeakerName(node);
            yield return BuildDialogue(textData.text);

            yield return Line_RunCommands(afterCommands);

            if (isSingleTimeAuto)
            {
                isSingleTimeAuto = false;
            }
            else if (!isAuto && !isSkip)
            {
                yield return WaitForUserInput();
                SoundManager.instance.PlayTextBoxSound();
            }
            else if (isSkip)
            {
                yield return new WaitForSeconds(0.2f);
                SoundManager.instance.PlayTextBoxSound();
            }
        }

        public List<Command> GetBeforeCommands(List<Command> commands)
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


        public IEnumerator Line_RunCommands(List<Command> commands)
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
                if (userPrompt || isSkip)
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