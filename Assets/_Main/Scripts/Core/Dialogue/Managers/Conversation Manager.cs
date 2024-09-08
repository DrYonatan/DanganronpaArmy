using COMMANDS;
using CHARACTERS;
using System.Collections;
using System.Collections.Generic;
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
                    string speakerName = "";
                    if (line.hasSpeaker)
                    {
                        speakerName = GetSpeakerEnglishName(line);
                        string direction = GetCharacterPosition(speakerName);
                        if (CharacterManager.instance.characters.ContainsKey(speakerName.ToLower()) && (direction == "left" || direction == "middle" || direction == "right"))
                            CameraManager.instance.MoveCamera(direction, 0.3f);
                          DecideCharactersToHide(speakerName);
                       

                    }

                    CharacterManager.instance.ShowCharacter(speakerName);
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


        public void DecideCharactersToHide(string speakerName)
        {
            if (GetCharacterPosition("Koby") == GetCharacterPosition(speakerName) && speakerName != "Koby" && speakerName != "Protagonist")
                CharacterManager.instance.HideCharacter("Koby");
            if (GetCharacterPosition("Noya") == GetCharacterPosition(speakerName) && speakerName != "Noya" && speakerName != "Protagonist")
                CharacterManager.instance.HideCharacter("Noya");
            if (GetCharacterPosition("Inbal") == GetCharacterPosition(speakerName) && speakerName != "Inbal" && speakerName != "Protagonist")
                CharacterManager.instance.HideCharacter("Inbal");
            if (GetCharacterPosition("Omer") == GetCharacterPosition(speakerName) && speakerName != "Omer" && speakerName != "Protagonist")
                CharacterManager.instance.HideCharacter("Omer");
            if (GetCharacterPosition("Noa") == GetCharacterPosition(speakerName) && speakerName != "Noa" && speakerName != "Protagonist")
                CharacterManager.instance.HideCharacter("Noa");
            if (GetCharacterPosition("Guy") == GetCharacterPosition(speakerName) && speakerName != "Guy" && speakerName != "Protagonist")
                CharacterManager.instance.HideCharacter("Guy");
            if (GetCharacterPosition("Kfir") == GetCharacterPosition(speakerName) && speakerName != "Kfir" && speakerName != "Protagonist")
                CharacterManager.instance.HideCharacter("Kfir");
            if (GetCharacterPosition("Maya") == GetCharacterPosition(speakerName) && speakerName != "Maya" && speakerName != "Protagonist")
                CharacterManager.instance.HideCharacter("Maya");
            if (GetCharacterPosition("Ariel") == GetCharacterPosition(speakerName) && speakerName != "Ariel" && speakerName != "Protagonist")
                CharacterManager.instance.HideCharacter("Ariel");
            if (GetCharacterPosition("Shiraz") == GetCharacterPosition(speakerName) && speakerName != "Shiraz" && speakerName != "Protagonist")
                CharacterManager.instance.HideCharacter("Shiraz");
            if (GetCharacterPosition("Liel") == GetCharacterPosition(speakerName) && speakerName != "Liel" && speakerName != "Protagonist")
                CharacterManager.instance.HideCharacter("Liel");
            if (GetCharacterPosition("Romi") == GetCharacterPosition(speakerName) && speakerName != "Romi" && speakerName != "Protagonist")
                CharacterManager.instance.HideCharacter("Romi");
            if (GetCharacterPosition("Roey") == GetCharacterPosition(speakerName) && speakerName != "Roey" && speakerName != "Protagonist")
                CharacterManager.instance.HideCharacter("Roey");
            if (GetCharacterPosition("Ohav") == GetCharacterPosition(speakerName) && speakerName != "Ohav" && speakerName != "Protagonist")
                CharacterManager.instance.HideCharacter("Ohav");
            if (GetCharacterPosition("Bamba") == GetCharacterPosition(speakerName) && speakerName != "Bamba" && speakerName != "Protagonist")
                CharacterManager.instance.HideCharacter("Bamba");


        }

        public string GetCharacterPosition(string speakerName)
        {
            string direction = "";
            if(CharacterManager.instance.characters.ContainsKey(speakerName.ToLower()))
            {
                if (GameObject.Find($"VN controller/Root/Canvas - Main/LAYERS/2 - Characters/Character - [{speakerName}]/Anim/Renderers/Later: 0").transform.position.x == GameObject.Find($"VN controller/Root/Canvas - Main/LAYERS/0 - Underlay Background/Middle").transform.position.x)
                    direction = "middle";
                else if (GameObject.Find($"VN controller/Root/Canvas - Main/LAYERS/2 - Characters/Character - [{speakerName}]/Anim/Renderers/Later: 0").transform.position.x == GameObject.Find($"VN controller/Root/Canvas - Main/LAYERS/0 - Underlay Background/Right").transform.position.x)
                    direction = "right";
                else
                    direction = "left";

            }
            Debug.Log($"The Direction is of {speakerName} is: " + direction);
            return direction;
        }
        public string GetSpeakerEnglishName(DIALOGUE_LINE line)
        {
            string speakerName = "";
            if (line.speakerData.name == "קובי")
                speakerName = "Koby";
            else if (line.speakerData.name == "נויה")
                speakerName = "Noya";
            else if (line.speakerData.name == "שירז")
                speakerName = "Shiraz";
            else if (line.speakerData.name == "ליאל")
                speakerName = "Liel";
            else if (line.speakerData.name == "אריאל")
                speakerName = "Ariel";
            else if (line.speakerData.name == "כפיר")
                speakerName = "Kfir";
            else if (line.speakerData.name == "נעה")
                speakerName = "Noa";
            else if (line.speakerData.name == "מאיה")
                speakerName = "Maya";
            else if (line.speakerData.name == "רועי")
                speakerName = "Roey";
            else if (line.speakerData.name == "עומר")
                speakerName = "Omer";
            else if (line.speakerData.name == "אוהב")
                speakerName = "Ohav";
            else if (line.speakerData.name == "גיא")
                speakerName = "Guy";
            else if (line.speakerData.name == "ענבל")
                speakerName = "Inbal";
            else if (line.speakerData.name == "רומי")
                speakerName = "Romi";
            else if (line.speakerData.name == "במבה")
                speakerName = "Bamba";
            else if (line.speakerData.name == "אלון")
                speakerName = "Protagonist";

            return speakerName;
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
    }
}
