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
        
        public ICharacterHandler characterHandler;
        public ConversationManager(TextArchitect architect, ICharacterHandler characterHandler)
        {
            this.architect = architect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
            this.characterHandler = characterHandler;
        }

        private void OnUserPrompt_Next()
        {
            userPrompt = true;
        }

        public Coroutine StartConversation(List<string> conversation)
        {
            StopConversation();

            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));

            return process;
        }

        public void StopConversation()
        {
            if (!isRunning)
                return;
            dialogueSystem.StopCoroutine(process);
            process = null;
            characterHandler?.OnStopConversation();
        }

        IEnumerator RunningConversation(List<string> conversation)
        {
            for(int i = 0; i < conversation.Count; i++)
            {
                //Don't show blank lines or run anything on them
                if (string.IsNullOrWhiteSpace(conversation[i]))
                    continue;
                DIALOGUE_LINE line = DialogueParser.Parse(conversation[i]);

                //Show Dialogue
                if(line.hasDialogue)
                {
                    // character handler will only exist in visual novel
                    characterHandler?.OnLineParsed(line);
                    yield return Line_RunDialogue(line);
                }
                //Run any commands
                if(line.hasCommands)
                {
                    yield return Line_RunCommands(line);
                }

                if(line.hasDialogue)
                //Wait for user input
                yield return WaitForUserInput();

            }
        }
        
        IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            //Show or hide the speaker name if there is one
            if (line.hasSpeaker)
            {
                dialogueSystem.ShowSpeakerName(line.speakerData.displayName);
            }
                 
            //Build Dialogue
            
            yield return BuildLineSegments(line.dialogueData);

        }

        IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            List<DL_COMMAND_DATA.Command> commands = line.commandData.commands;

            foreach(DL_COMMAND_DATA.Command command in commands)
            {
                if (command.waitForCompletion)
                    yield return CommandManager.instance.Execute(command.name, command.arguments);
                else
                CommandManager.instance.Execute(command.name, command.arguments);
            }

            yield return null;
        }

        IEnumerator BuildLineSegments(DL_DIALOGUE_DATA line)
        {
            for(int i = 0; i < line.segments.Count; i++)
            {
                DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment = line.segments[i];

                yield return WaitForDialogueSegmentSignalToBeTriggered(segment);

                yield return BuildDialogue(segment.dialogue, segment.appendText);
            }
        }

        IEnumerator WaitForDialogueSegmentSignalToBeTriggered(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment)
        {
            switch(segment.startSignal)
            {
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.C:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.A:
                    yield return WaitForUserInput();
                    break;
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WC:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WA:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;
                default:
                    break;
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
                    if (!architect.hurryUp)
                        architect.hurryUp = true;
                    else
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
            if(!architect.isBuilding)
                SoundManager.instance.PlayTextBoxSound();
        }

        public void ClearTextBox()
        {
            architect.Clear();
        }
    }
}
