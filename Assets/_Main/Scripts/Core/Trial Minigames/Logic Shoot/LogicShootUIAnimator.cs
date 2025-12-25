using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class LogicShootUIAnimator : MonoBehaviour
{
    public TextMeshProUGUI mikbazTextPrefab;
    public RectTransform holePrefab;
    public ShootTarget targetPrefab;
    public ShootTarget finalTargetPrefab;
    public TextShatterExplosion successExplosion;
    public Explosion failExplosion;

    public AudioClip music;
    public AudioClip finalTargetSound;
    public AudioClip finalTargetMusic;
    public AudioClip stopGameSound;

    public RectTransform playerBar;
    public Image playerHp;

    public RectTransform enemyBar;
    public Image enemyHp;

    public TextMeshProUGUI finalQuestionText;

    public RectTransform timersContainer;
    public RectTransform targetsContainer;
    public TextMeshProUGUI ammoNumberText;
    public Button switchStacksButton;
    public Image stopGameTimer;
    public Image stopGameCooldownTimer;
    public Image stopGameCooldownGlow;
    public Image stopGameHalo;
    public PostProcessVolume colorGrading;

    public MinigameStartAnimation startAnimation;

    public RectTransform stacksContainer;
    public List<Image> stacks;

    public List<ShootTarget> activeTargets;

    public NowIUnderstandAnimator faceCloseup;
    public ScreenShatterManager screenShatter;

    public GameObject stopGamePostProcessing;
    
    public Image smoke;

    private Coroutine colorGradingRoutine;

    public void Initialize()
    {
        foreach (Image stack in stacks)
        {
            stack.color = Color.white;
        }

        Color color = stopGameHalo.color;
        color.a = 0f;
        stopGameHalo.color = color;
        playerBar.anchoredPosition -= new Vector2(1000, 0);
        enemyBar.anchoredPosition += new Vector2(1000, 0);
        stacksContainer.anchoredPosition -= new Vector2(0, 500);
        timersContainer.localRotation = Quaternion.Euler(90, 0, 0);
    }

    public void ShowMikbazText(int number)
    {
        TextMeshProUGUI mikbazText = Instantiate(mikbazTextPrefab, enemyHp.transform);
        mikbazText.rectTransform.anchoredPosition = new Vector2(-535, -100);
        mikbazText.text = "מקבץ " + number + " !!";

        Sequence seq = DOTween.Sequence();
        seq.Append(mikbazText.DOFade(0f, 0.05f).SetLoops(6, LoopType.Yoyo));
        seq.Append(mikbazText.rectTransform.DOAnchorPos(enemyHp.rectTransform.anchoredPosition, 1f)
            .SetEase(Ease.InCubic));
        seq.Join(mikbazText.rectTransform.DOScale(0.1f, 1f).SetEase(Ease.InCubic));

        seq.OnComplete(() => OnMikbazFinish(mikbazText.gameObject, (10f - number) / 5f));
    }

    private void OnMikbazFinish(GameObject mikbaz, float amountToDamage)
    {
        Destroy(mikbaz);
        LogicShootManager.instance.DamageEnemy(amountToDamage);
    }

    public IEnumerator GenerateTargets(List<ShootTargetData> shootTargets)
    {
        float duration = LogicShootManager.instance.GetMaxDuration(shootTargets);

        foreach (ShootTargetData target in shootTargets)
        {
            ShootTarget newTarget = Instantiate(targetPrefab, targetsContainer);

            newTarget.questionText.text = target.question;
            newTarget.timeOut = target.timeOut;
            newTarget.GetComponent<RectTransform>().anchoredPosition = target.spawnPosition;
            newTarget.targetPosition = target.targetPosition;
            newTarget.movementTime = target.movementTime;

            for (int i = 0; i < newTarget.areas.Count; i++)
            {
                newTarget.areas[i].isCorrect = target.answers[i].isCorrect;
                newTarget.areas[i].answer.text = target.answers[i].answer;
            }

            newTarget.LifeTime();

            activeTargets.Add(newTarget);
        }

        yield return new WaitForSeconds(duration);

        activeTargets.Clear();
    }

    public void ShowUI()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(playerBar.DOAnchorPosX(playerBar.anchoredPosition.x + 1000, 0.2f));
        seq.Join(enemyBar.DOAnchorPosX(enemyBar.anchoredPosition.x - 1000, 0.2f));
        seq.Join(stacksContainer.DOAnchorPosY(stacksContainer.anchoredPosition.y + 500, 0.2f));
        seq.Append(timersContainer.DOLocalRotate(Vector3.zero, 0.2f));
    }

    public void PlayStartAnimation()
    {
        MinigameStartAnimation anim = Instantiate(startAnimation, TrialManager.instance.globalUI);
        anim.Animate(0f);
    }

    public void UpdateAmmo(int currentAmmo)
    {
        string pre = currentAmmo < 10 ? "0" : "";
        ammoNumberText.text = pre + currentAmmo;
    }

    public void UpdatePlayerHp(float currentHp)
    {
        playerHp.fillAmount = currentHp / TrialManager.instance.barsAnimator.fullHpImageDivideAmount;
    }

    public void RemoveStack(int index)
    {
        stacks[index].DOColor(Color.black, 0.1f).SetUpdate(true);
    }

    public IEnumerator ShowFinalQuestion(ShootTargetData data)
    {
        foreach (ShootTarget target in activeTargets)
        {
            if (target != null)
            {
                target.transform.DOKill();
                target.canvasGroup.DOFade(0f, 0.1f).OnComplete(() => Destroy(target));
            }
        }

        activeTargets.Clear();
        MusicManager.instance.StopSong();
        SoundManager.instance.PlaySoundEffect(finalTargetSound);
        CameraController.instance.cameraTransform.DOKill();
        CameraController.instance.cameraTransform.localRotation = Quaternion.Euler(0f, 0f, 20f);
        CameraController.instance.cameraTransform.localPosition = new Vector3(0,
            LogicShootManager.instance.characterStand.heightPivot.position.y, -3);
        CameraController.instance.cameraTransform.DOLocalMove(
            CameraController.instance.cameraTransform.localPosition + new Vector3(0, 0, 2), 4f);
        CameraController.instance.cameraTransform.DOLocalRotate(Vector3.zero, 4f);

        yield return new WaitForSeconds(0.2f);

        finalQuestionText.text = data.question;
        finalQuestionText.DOFade(1f, 0.2f);

        yield return new WaitForSeconds(1f);

        ShootTarget finalTarget = Instantiate(finalTargetPrefab, targetsContainer);
        for (int i = 0; i < finalTarget.areas.Count; i++)
        {
            finalTarget.areas[i].isCorrect = data.answers[i].isCorrect;
            finalTarget.areas[i].answer.text = data.answers[i].answer;
        }

        finalTarget.timeOut = data.timeOut;
        finalTarget.GetComponent<RectTransform>().anchoredPosition = data.spawnPosition;
        finalTarget.targetPosition = data.targetPosition;
        finalTarget.movementTime = data.movementTime;
        finalTarget.canvasGroup.alpha = 0f;
        finalTarget.LifeTime();

        finalTarget.transform.DOShakePosition(
            duration: 1f,
            strength: 10f,
            vibrato: 30,
            randomness: 90f,
            snapping: false,
            fadeOut: false
        ).SetEase(Ease.Linear).SetLoops(-1).SetRelative().SetLink(finalTarget.gameObject);

        finalTarget.canvasGroup.DOFade(1f, 0.2f);
        finalTarget.transform.DOScale(1.5f, finalTarget.timeOut).SetLink(finalTarget.gameObject);

        colorGradingRoutine = StartCoroutine(ColorGrading(3f, 1f));

        yield return new WaitForSeconds(data.timeOut);

        if (LogicShootManager.instance.isActive)
            LogicShootManager.instance.ReturnToGame();
    }

    public void StopShowingFinalQuestion()
    {
        StopCoroutine(colorGradingRoutine);
        finalQuestionText.DOFade(0f, 0.2f);
    }

    public void RotateTimersContainer(float angle)
    {
        timersContainer.DOLocalRotate(new Vector3(0, angle, 0), 0.4f).SetUpdate(true);
    }

    private void StopGameTimer()
    {
        stopGameTimer.color = Color.green;
        stopGameTimer.fillAmount = 1f;
        stopGameTimer.DOFillAmount(0f, 7f).SetUpdate(true);
        stopGameTimer.DOColor(Color.yellow, 3.5f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
            stopGameTimer.DOColor(Color.red, 3.5f).SetEase(Ease.Linear).SetUpdate(true));
    }

    public void StopGameCooldown()
    {
        stopGameCooldownTimer.fillAmount = 0f;

        Sequence seq = DOTween.Sequence();
        seq.Append(stopGameCooldownTimer.DOFillAmount(1f, LogicShootManager.instance.stopGameCooldownTime)
            .SetEase(Ease.Linear));
        seq.Append(stopGameCooldownGlow.DOFade(1f, 0.2f).SetLoops(2, LoopType.Yoyo));
    }

    public IEnumerator FinishAnimation()
    {
        faceCloseup.gameObject.SetActive(true);
        yield return faceCloseup.Show();
        faceCloseup.gameObject.SetActive(false);

        StopCoroutine(colorGradingRoutine);
        colorGrading.weight = 0f;

        CharacterStand characterStand = LogicShootManager.instance.characterStand;
        characterStand.SetSprite(characterStand.character.FindStateByName("scared"));
        yield return FinishCameraMovement();

        ScreenShatterManager shatter = Instantiate(screenShatter);
        yield return shatter.ScreenShatter();

        TrialManager.instance.FadeCharactersExcept(LogicShootManager.instance.segment.character, 1f, 0f);
    }

    private IEnumerator FinishCameraMovement()
    {
        CameraController.instance.cameraTransform.DOKill();

        CameraController.instance.cameraTransform.localPosition = new Vector3(0, 4.3f, -5.4f);
        CameraController.instance.cameraTransform.localRotation = Quaternion.Euler(0, 0, 20);

        yield return new WaitForSeconds(0.5f);

        CameraController.instance.cameraTransform.localPosition = new Vector3(0, 4.3f, -7f);
        CameraController.instance.cameraTransform.localRotation = Quaternion.Euler(0, 0, -20);

        yield return new WaitForSeconds(0.5f);

        CameraController.instance.cameraTransform.localPosition = new Vector3(0, 4.3f, -9f);
        CameraController.instance.cameraTransform.localRotation = Quaternion.Euler(0, 0, 10);
    }

    private void HaloAnimation()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(stopGameHalo.DOFade(1f, 0f).SetUpdate(true));
        seq.Append(stopGameHalo.rectTransform.DOScale(1f, 0f).SetUpdate(true));
        seq.Append(stopGameHalo.DOFade(0f, 0.5f).SetUpdate(true));
        seq.Join(stopGameHalo.rectTransform.DOScale(15f, 0.5f).SetUpdate(true));
        seq.SetUpdate(true);
    }

    public void RaiseRifleAnimation()
    {
        SoundManager.instance.PlaySoundEffect(stopGameSound);
        HaloAnimation();
        StartCoroutine(StopGameColorGrading(0.5f));
        RotateTimersContainer(90);
        StopGameTimer();
        stopGamePostProcessing.SetActive(true);
    }

    private IEnumerator ColorGrading(float duration, float to)
    {
        float elapsedTime = 0f;
        float from = colorGrading.weight;

        while (elapsedTime < duration)
        {
            colorGrading.weight = Mathf.Lerp(from, to, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    private IEnumerator StopGameColorGrading(float duration)
    {
        float actualDuration = duration / 2;
        yield return ColorGrading(actualDuration, 1f);
        yield return ColorGrading(actualDuration, 0f);
    }

    public void TargetFailExplosion(Vector2 position)
    {
        Explosion explosion = Instantiate(failExplosion, targetsContainer);
        explosion.rectTransform.anchoredPosition = position;
        explosion.Explode();
    }
}