using System.Collections;
using System.Collections.Generic;
using DIALOGUE;

public class ComicText : ComicStep
{
    public List<DialogueNode> dialogueNodes = new List<DialogueNode>(); 
    public IEnumerator Play()
    {
        foreach (DialogueNode node in dialogueNodes)
        {
            yield return DialogueSystem.instance.Say(node);
        }
    }
}
