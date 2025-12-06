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
        enemyHP = 10;
        ammo = 30;

        StartCoroutine(PlayGame());
    }

    private IEnumerator PlayGame()
    {
        CharacterCutIn cutIn = Instantiate(segment.cutIn, TrialManager.instance.globalUI);
        cutIn.Animate();
        yield return new WaitForSeconds(3f);
        
        ImageScript.instance.FadeToBlack(0.2f);
        yield return CameraController.instance.DiscussionOutroMovement(2.5f);
        ImageScript.instance.UnFadeToBlack(0.2f);
        yield return CameraController.instance.DescendingCircling(2f);

        CharacterStand characterStand =
            TrialManager.instance.characterStands.Find(stand => stand.character == segment.character);
        
        yield return CameraController.instance.SpinToTarget(characterStand.transform, characterStand.heightPivot, Vector3.zero,
            Vector3.zero, 0f);
        
        animator.PlayStartAnimation();
        yield return CameraController.instance.MoveAndRotate(new Vector3(0, 0, 1f), Vector3.zero, 2.5f);
        
        TrialManager.instance.FadeCharactersExcept(segment.character, 0f);
        animator.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        
        TrialCursorManager.instance.Show();
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
        TrialManager.instance.barsAnimator.DecreaseHealthFromMeter(animator.enemyHpBar, amount, 0.2f);
        
        if (enemyHP <= 0)
        {
            FinishGame();
        }
    }
}