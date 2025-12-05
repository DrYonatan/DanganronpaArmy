using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicShootManager : MonoBehaviour
{
    public static LogicShootManager instance { get; private set; }

    public LogicShootUIAnimator animator;

    public LogicShootSegment segment;
    
    public float enemyHP;
    public float ammo;

    public bool coolDown;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    void Update()
    {
        TrialCursorManager.instance.ReticleAsCursor();
        if (Input.GetMouseButtonDown(0) && !coolDown)
        {
            ammo--;
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        coolDown = true;
        
        yield return new WaitForSeconds(0.2f);
        
        coolDown = false;
    }

    public void Play(LogicShootSegment newSegment)
    {
        segment = newSegment;
        animator.gameObject.SetActive(true);
        CharacterCutIn cutIn = Instantiate(segment.cutIn, animator.transform);
        cutIn.Animate();
        TrialCursorManager.instance.Show();
        enemyHP = 10;
        MusicManager.instance.PlaySong(animator.music);
        ammo = 30;
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