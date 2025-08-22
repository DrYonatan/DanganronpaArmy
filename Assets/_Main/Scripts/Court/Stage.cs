using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Court;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Stage")]
public class Stage : TrialSegment
{
    public Evidence[] evidences = new Evidence[5];
    public AudioClip audioClip;
    public List<DebateNode> dialogueNodes;
    
    public List<DiscussionNode> finishNodes;

    public override void Play()
    {
        GameLoop.instance.PlayDebate(this);
    }
}
