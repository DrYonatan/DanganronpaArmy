using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TargetAreaAnswer
{
    public string answer;
    public bool isCorrect;
}

[Serializable]
public class ShootTargetData
{
    public string question;
    public List<TargetAreaAnswer> answers;
    public float timeOut;
    public Vector2 spawnPosition;
    public Vector2 targetPosition;
    public float movementTime = 3f;
}

[Serializable]
public class ShootTargetsStage
{
    public Vector3 cameraStartPosition;
    public List<ShootTargetData> targets;
}

[CreateAssetMenu(menuName = "Data/Logic Shoot")]
public class LogicShootSegment : TrialSegment
{
    public List<ShootTargetsStage> stages;
    public Character character;
    
    public override void Play()
    {
        LogicShootManager.instance.Play(this);
    }
}