using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LogicShootManager : MonoBehaviour
{
    public static LogicShootManager instance { get; private set; }

    public LogicShootUIAnimator animator;

    public LogicShootSegment segment;

    public GraphicRaycaster raycaster;

    public AnimationCurve cameraCurve;

    public RifleManager rifleManager;

    public CharacterStand characterStand;

    public float enemyHP;

    public bool shootCooldown;

    public float stopGameCooldownTime = 10f;
    public bool stopGameCooldown;

    private bool isRifleUp;

    public bool isActive;
    public bool isInFinish;

    private Coroutine finalTargetRoutine;
    private bool isFinishedTargets;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    void Update()
    {
        if(!isActive)
            return;
        
        TrialCursorManager.instance.ReticleAsCursor();
        if (Input.GetMouseButtonDown(0) && !isRifleUp && !shootCooldown)
        {
            if(rifleManager.IsRifleIntact())
               Shoot();
            else
            {
                SoundManager.instance.PlaySoundEffect(rifleManager.errorSound);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isRifleUp && !stopGameCooldown)
            {
                RaiseRifle();
            }
            else if (isRifleUp)
            {
                PutDownRifle();
            }
        }
    }

    private void RaiseRifle()
    {
        isRifleUp = true;
        Time.timeScale = 0f;
        rifleManager.RaiseRifle();
        MusicManager.instance.LowerVolume();
        animator.RaiseRifleAnimation();
        StartCoroutine(RifleCountDown());
    }

    private IEnumerator RifleCountDown()
    {
        yield return new WaitForSecondsRealtime(5f);
        if (isRifleUp)
            PutDownRifle();
    }

    private void PutDownRifle()
    {
        isRifleUp = false;
        Time.timeScale = 1f;
        rifleManager.PutRifleDown();
        MusicManager.instance.RaiseVolume();
        animator.RotateTimersContainer(0);
        animator.StopGameCooldown();
        animator.stopGamePostProcessing.SetActive(false);
        StartCoroutine(StopGameCooldown());
    }

    private void Shoot()
    {
        SoundManager.instance.PlaySoundEffect(rifleManager.shotSound);
        rifleManager.ammo--;
        animator.UpdateAmmo(rifleManager.ammo);
        if (rifleManager.ammo == 0)
            rifleManager.OutOfAmmo();
        ProcessShot();
        StartCoroutine(CoolDown());

        RandomizeRifleStuck();
    }

    private void RandomizeRifleStuck()
    {
        if (rifleManager.rifleErrorCooldown)
            return;

        int number = Random.Range(1, 30);

        switch (number)
        {
            case 1:
                rifleManager.RifleStuckTypeOne();
                rifleManager.rifleErrorCooldown = true;
                break;
            case 2:
                rifleManager.RifleStuckTypeTwo();
                rifleManager.rifleErrorCooldown = true;
                break;
        }
    }

    private void ProcessShot()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult r in results)
        {
            var go = r.gameObject;

            ShootTarget target = go.GetComponent<ShootTarget>();
            ShootTargetArea area = go.GetComponent<ShootTargetArea>();

            if (target != null || area != null)
            {
                RectTransform hole = CreateHole(go);

                if (area != null)
                {
                    area.holes.Add(hole);
                    area.CheckShoot();
                    return;
                }
            }
        }
    }

    private RectTransform CreateHole(GameObject go)
    {
        RectTransform rt = go.GetComponent<RectTransform>();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rt,
            Input.mousePosition,
            CameraController.instance.uiCamera,
            out localPoint
        );

        RectTransform hole = Instantiate(animator.holePrefab, go.transform);
        hole.anchoredPosition = localPoint;

        Image smoke = Instantiate(animator.smoke, go.transform);
        smoke.rectTransform.anchoredPosition = localPoint;
        smoke.rectTransform.DOAnchorPosY(smoke.rectTransform.anchoredPosition.y + 50f, 0.5f).SetLink(smoke.gameObject);
        smoke.DOFade(0f, 0.5f).SetLink(smoke.gameObject);
        smoke.rectTransform.DOScaleY(1.5f, 0.5f).SetLink(smoke.gameObject).OnComplete(() => Destroy(smoke.gameObject));

        return hole;
    }

    private IEnumerator CoolDown()
    {
        shootCooldown = true;

        yield return new WaitForSeconds(0.2f);

        shootCooldown = false;
    }

    private IEnumerator StopGameCooldown()
    {
        stopGameCooldown = true;
        
        yield return new WaitForSeconds(stopGameCooldownTime);

        rifleManager.rifleErrorCooldown = false;
        stopGameCooldown = false;
    }

    public void Play(LogicShootSegment newSegment)
    {
        segment = newSegment;
        enemyHP = 10;

        rifleManager.InitializeRifle();

        animator.UpdatePlayerHp(TrialManager.instance.playerStats.hp);
        animator.UpdateAmmo(rifleManager.ammo);
        rifleManager.PutRifleDown();

        StartCoroutine(PlayGame());
    }

    private IEnumerator PlayGame()
    {
        ImageScript.instance.FadeToBlack(0.2f);
        yield return CameraController.instance.DiscussionOutroMovement(2.5f);
        animator.gameObject.SetActive(true);
        animator.Initialize();

        ImageScript.instance.UnFadeToBlack(0.2f);
        yield return CameraController.instance.DescendingCircling(2f);

        characterStand =
            TrialManager.instance.characterStands.Find(stand => stand.character == segment.character);

        yield return CameraController.instance.SpinToTarget(characterStand.transform, characterStand.heightPivot,
            Vector3.zero,
            Vector3.zero, 0f);

        animator.PlayStartAnimation();
        yield return CameraController.instance.MoveAndRotate(new Vector3(0, 0, 1f), Vector3.zero, 2.5f);
        animator.ShowUI();
        
        TrialManager.instance.FadeCharactersExcept(segment.character, 0f, 0.5f);

        yield return new WaitForSeconds(0.5f);

        TrialCursorManager.instance.Show();
        MusicManager.instance.PlaySong(animator.music);

        isActive = true;

        StartCoroutine(PlayStages(segment.stages));
    }

    private IEnumerator PlayStages(List<ShootTargetsStage> stages)
    {
        isFinishedTargets = false;
        while (isActive && !isInFinish)
        {
            foreach (ShootTargetsStage stage in stages)
            {
                if (!isActive || isInFinish)
                    break;
                MoveCameraToCenter(stage.cameraStartPosition, GetMaxDuration(stage.targets));
                CameraController.instance.cameraTransform.localRotation = Quaternion.Euler(stage.rotation);
                characterStand.SetSprite(characterStand.character.FindStateByName(stage.emotion));

                yield return animator.GenerateTargets(stage.targets);
            }
        }

        isFinishedTargets = true;
    }

    public void FinishGame()
    {
        StartCoroutine(FinishPipeline());
    }

    private IEnumerator FinishPipeline()
    {
        animator.finalQuestionText.DOFade(0f, 0.1f);
        isActive = false;
        TrialCursorManager.instance.Hide();
        yield return animator.FinishAnimation();

        ImageScript.instance.UnFadeToBlack(0.2f);
        animator.gameObject.SetActive(false);
        CharacterStand stand = TrialManager.instance.characterStands.Find(stand => stand.character.isProtagonist);
        CameraController.instance.TeleportToTarget(stand.transform,
            stand.heightPivot, Vector3.zero, Vector3.zero, 0f);
        yield return CameraController.instance.FovOutro();
        segment.Finish();
    }

    public void DamageEnemy(float amount)
    {
        enemyHP -= amount;
        TrialManager.instance.barsAnimator.DecreaseHealthFromMeter(animator.enemyHp, enemyHP, 0.2f);

        if (enemyHP <= 0)
        {
            FinalQuestion();
        }
    }

    public void DamagePlayer(float amount)
    {
        SoundManager.instance.PlaySoundEffect(TrialManager.instance.barsAnimator.damageSound);
        TrialManager.instance.DecreaseHealthFromMeter(animator.playerHp, amount);
    }

    private void MoveCameraToCenter(Vector3 from, float duration)
    {
        Vector3 originalPosition = CameraController.instance.cameraTransform.localPosition;

        CameraController.instance.cameraTransform.localPosition += from;

        CameraController.instance.cameraTransform
            .DOLocalMove(originalPosition, duration).SetEase(cameraCurve);
    }

    private void FinalQuestion()
    {
        isInFinish = true;
        finalTargetRoutine = StartCoroutine(animator.ShowFinalQuestion(segment.finalTarget));
    }

    public void ReturnToGame()
    {
        StopCoroutine(finalTargetRoutine);
        animator.colorGrading.weight = 0f;
        animator.StopShowingFinalQuestion();
        MusicManager.instance.PlaySong(animator.music);
        isInFinish = false;
        enemyHP = 1f;
        animator.enemyHp.fillAmount = 1f / 10f;
        if(isFinishedTargets)
            StartCoroutine(PlayStages(segment.stages));
    }

    public float GetMaxDuration(List<ShootTargetData> shootTargets)
    {
        if (shootTargets == null || shootTargets.Count == 0)
            return 0f; // or throw an exception, depending on your design

        float max = float.MinValue;

        foreach (ShootTargetData target in shootTargets)
        {
            if (target.timeOut > max)
                max = target.timeOut;
        }

        return max;
    }
}