using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Court;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Stage")]
public class Stage : ScriptableObject
{
    public Evidence[] evidences = new Evidence[5];
    public AudioClip audioClip;
    public List<DebateDialogueNode> dialogueNodes;
}
