using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Stage")]
public class Stage : ScriptableObject
{
    public Evidence[] evidences = new Evidence[5];
    public AudioClip audioClip;
    public List<DialogueNode> dialogueNodes;
}
