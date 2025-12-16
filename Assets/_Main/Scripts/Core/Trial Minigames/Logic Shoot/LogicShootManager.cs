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

    public float enemyHP;

    public bool shootCooldown;

    public float stopGameCooldownTime = 10f;
    public bool stopGameCooldown;

    private bool isRifleUp;

    public bool isActive;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    void Update()
    {
        TrialCursorManager.instance.ReticleAsCursor();
        if (Input.GetMouseButtonDown(0) && !isRifleUp && !shootCooldown && rifleManager.IsRifleIntact())
        {
            Shoot();
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
        StartCoroutine(animator.ShowBlackAndWhite());
        StartCoroutine(RifleCountDown());
    }

    private IEnumerator RifleCountDown()
    {
        yield return new WaitForSecondsRealtime(5f);
        PutDownRifle();
    }

    private void PutDownRifle()
    {
        isRifleUp = false;
        Time.timeScale = 1f;
        rifleManager.PutRifleDown();
        StartCoroutine(StopGameCooldown());
        animator.blackAndWhiteScreenOverlay.DOFade(0f, 0f).SetUpdate(true);
    }

    private void Shoot()
    {
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

        int number = Random.Range(1, 20);

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
            CameraController.instance.camera,
            out localPoint
        );

        RectTransform hole = Instantiate(animator.holePrefab, go.transform);
        hole.anchoredPosition = localPoint;

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
        ImageScript.instance.UnFadeToBlack(0.2f);
        yield return CameraController.instance.DescendingCircling(2f);

        CharacterStand characterStand =
            TrialManager.instance.characterStands.Find(stand => stand.character == segment.character);

        yield return CameraController.instance.SpinToTarget(characterStand.transform, characterStand.heightPivot,
            Vector3.zero,
            Vector3.zero, 0f);

        animator.Initialize();
        animator.PlayStartAnimation();
        yield return CameraController.instance.MoveAndRotate(new Vector3(0, 0, 1f), Vector3.zero, 2.5f);

        TrialManager.instance.FadeCharactersExcept(segment.character, 0f);
        animator.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        TrialCursorManager.instance.Show();
        MusicManager.instance.PlaySong(animator.music);

        isActive = true;

        StartCoroutine(PlayTargets(segment.stages));
    }

    private IEnumerator PlayTargets(List<ShootTargetsStage> stages)
    {
        while (isActive)
        {
            foreach (ShootTargetsStage stage in stages)
            {
                if (!isActive)
                    break;
                MoveCameraToCenter(stage.cameraStartPosition);
                yield return animator.GenerateTargets(stage.targets);
            }
        }
    }

    public IEnumerator FinishGame()
    {
        animator.finalQuestionText.DOFade(0f, 0.1f);
        yield return new WaitForSeconds(2f);

        StartCoroutine(FinishPipeLine());
    }

    private IEnumerator FinishPipeLine()
    {
        yield return null;
        TrialManager.instance.FadeCharactersExcept(segment.character, 1f);
        animator.gameObject.SetActive(false);
        TrialCursorManager.instance.Hide();
        segment.Finish();
    }

    public void DamageEnemy(float amount)
    {
        enemyHP -= amount;
        TrialManager.instance.barsAnimator.DecreaseHealthFromMeter(animator.enemyHpBar, enemyHP, 0.2f);

        if (enemyHP <= 0)
        {
            FinalQuestion();
        }
    }

    public void DamagePlayer(float amount)
    {
        TrialManager.instance.DecreaseHealthFromMeter(animator.playerHpBar, amount);
    }

    private void MoveCameraToCenter(Vector3 from)
    {
        Vector3 originalPosition = CameraController.instance.cameraTransform.localPosition;

        CameraController.instance.cameraTransform.localPosition += from;

        CameraController.instance.cameraTransform
            .DOLocalMove(originalPosition, 3f).SetEase(cameraCurve);
    }

    private void FinalQuestion()
    {
        isActive = false;
        StartCoroutine(animator.ShowFinalQuestion(segment.finalTarget));
    }

    public void ReturnToGame()
    {
        animator.StopShowingFinalQuestion();
        isActive = true;
        enemyHP = 1f;
        animator.enemyHpBar.fillAmount = 1f / 10f;
    }
}