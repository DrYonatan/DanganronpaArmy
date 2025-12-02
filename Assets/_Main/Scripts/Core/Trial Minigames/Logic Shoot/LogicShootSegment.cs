using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShootTargetData
{
    public string question;
    public List<string> answers;
    
}

[CreateAssetMenu(menuName = "Data/Logic Shoot")]
public class LogicShootSegment : TrialSegment
{
    public override void Play()
    {
        LogicShootManager.instance.Play(this);
    }
}