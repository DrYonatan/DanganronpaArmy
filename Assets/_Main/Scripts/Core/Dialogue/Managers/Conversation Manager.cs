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
        
        public IConversationNodePlayer conversationNodePlayer;

        public ConversationManager(TextArchitect architect, IConversationNodePlayer worldHandler)
        {
            this.architect = architect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
            this.conversationNodePlayer = worldHandler;
        }

        private void OnUserPrompt_Next()
        {
            userPrompt = true;
        }

        public Coroutine StartConversation(List<DialogueNode> nodes)
        {
            StopConversation();

            process = dialogueSystem.StartCoroutine(RunningConversation(nodes));

            return process;
        }

        public void StopConversation()
        {
            if (!isRunning)
                return;
            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunningConversation(List<DialogueNode> nodes)
        {

            
            for(int i = 0; i < nodes.Count; i++)
            {
                VNTextData textData = nodes[i].textData as VNTextData;
                
               // characterHandler?.OnLineParsed(textData);
               DialogueSystem.instance.ShowSpeakerName(nodes[i].character.displayName);
                conversationNodePlayer.PlayConversationNode(i);
                yield return BuildDialogue(textData.text);
                //Run any commands
                yield return Line_RunCommands(textData.commands);
                yield return WaitForUserInput();

            }

            conversationNodePlayer.HandleConversationEnd();
        }
        

        IEnumerator Line_RunCommands(List<Command> commands)
        {
            foreach (Command command in commands)
            {
                command.Execute();
                yield return null;
            }
            
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
