using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicShootManager : MonoBehaviour
{
    public static LogicShootManager instance { get; private set; }

    public LogicShootUIAnimator animator;

    public LogicShootSegment segment;
    
    public float enemyHP;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    void Update()
    {
        TrialCursorManager.instance.ReticleAsCursor();
    }

    public void Play(LogicShootSegment newSegment)
    {
        segment = newSegment;
        animator.gameObject.SetActive(true);
        TrialCursorManager.instance.Show();
        enemyHP = 10;
        MusicManager.instance.PlaySong(animator.music);
        StartCoroutine(PlayTargets(segment.stages));
    }

    private IEnumerator PlayTargets(List<ShootTargetsStage> stages)
    {
        foreach (ShootTargetsStage stage in stages)
        {
            yield return animator.GenerateTargets(stage.targets);
        }
    }

    private void FinishGame()
    {
        segment.Finish();
    }

    public void DamageEnemy(float amount)
    {
        enemyHP -= amount;

        if (enemyHP <= 0)
        {
            FinishGame();
        }
    }
}