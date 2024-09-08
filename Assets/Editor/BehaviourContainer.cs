using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviour Editor/Container")]
public class BehaviourContainer : ScriptableObject
{
    public List<DialogueNode> nodes;
}
