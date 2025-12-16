using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogicShootUIAnimator : MonoBehaviour
{
    public TextMeshProUGUI mikbazTextPrefab;
    public RectTransform holePrefab;
    public ShootTarget targetPrefab;
    public ShootTarget finalTargetPrefab;

    public AudioClip music;

    public Image playerHpBar;
    public Image enemyHpBar;
    public TextMeshProUGUI finalQuestionText;

    public RectTransform timersContainer;
    public RectTransform targetsContainer;
    public TextMeshProUGUI ammoNumberText;
    public Button switchStacksButton;
    public Image stopGameTimer;

    public MinigameStartAnimation startAnimation;

    public List<Image> stacks;

    public RawImage blackAndWhiteScreenOverlay;

    public List<ShootTarget> activeTargets;

    public NowIUnderstandAnimator faceCloseup;
    public ScreenShatterManager screenShatter;

    public void Initialize()
    {
        foreach (Image stack in stacks)
        {
            stack.color = Color.white;
        }
    }

    public void ShowMikbazText(int number)
    {
        TextMeshProUGUI mikbazText = Instantiate(mikbazTextPrefab, transform);
        mikbazText.text = "מקבץ " + number + " !!";

        Sequence seq = DOTween.Sequence();
        seq.Append(mikbazText.DOFade(0f, 0.05f).SetLoops(6, LoopType.Yoyo));
        seq.Append(mikbazText.rectTransform.DOMove(enemyHpBar.rectTransform.position, 1f)
            .SetEase(Ease.InCubic));
        seq.Join(mikbazText.rectTransform.DOScale(0.1f, 1f).SetEase(Ease.InCubic));

        seq.OnComplete(() => OnMikbazFinish(mikbazText.gameObject, (10f - number) / 5f));
    }

    private void OnMikbazFinish(GameObject mikbaz, float amountToDamage)
    {
        Destroy(mikbaz);
        LogicShootManager.instance.DamageEnemy(amountToDamage);
    }

    private float GetMaxDuration(List<ShootTargetData> shootTargets)
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

    public IEnumerator GenerateTargets(List<ShootTargetData> shootTargets)
    {
        float duration = GetMaxDuration(shootTargets);
        foreach (ShootTargetData target in shootTargets)
        {
            ShootTarget newTarget = Instantiate(targetPrefab, targetsContainer);
            for (int i = 0; i < newTarget.areas.Count; i++)
            {
                newTarget.questionText.text = target.question;
                newTarget.areas[i].isCorrect = target.answers[i].isCorrect;
                newTarget.areas[i].answer.text = target.answers[i].answer;
                newTarget.timeOut = target.timeOut;
                newTarget.GetComponent<RectTransform>().anchoredPosition = target.spawnPosition;
                newTarget.targetPosition = target.targetPosition;
                newTarget.movementTime = target.movementTime;
                newTarget.LifeTime();
            }

            activeTargets.Add(newTarget);
        }

        yield return new WaitForSeconds(duration);

        activeTargets.Clear();
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
        playerHpBar.fillAmount = currentHp / TrialManager.instance.barsAnimator.fullHpImageDivideAmount;
    }

    public void RemoveStack(int index)
    {
        stacks[index].DOColor(Color.black, 0.1f).SetUpdate(true);
    }

    public IEnumerator ShowBlackAndWhite()
    {
        yield return new WaitForEndOfFrame();
        Texture2D screenShot = ScreenCapture.CaptureScreenshotAsTexture();
        Texture2D newScreenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        newScreenShot.SetPixels(screenShot.GetPixels());
        newScreenShot.Apply();

        blackAndWhiteScreenOverlay.texture = newScreenShot;
        blackAndWhiteScreenOverlay.DOFade(1f, 0f).SetUpdate(true);
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

        yield return new WaitForSeconds(0.5f);

        finalQuestionText.text = data.question;
        finalQuestionText.DOFade(1f, 0.2f);

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
        finalTarget.LifeTime();

        yield return new WaitForSeconds(data.timeOut);

        if(LogicShootManager.instance.isActive)
           LogicShootManager.instance.ReturnToGame();
    }

    public void StopShowingFinalQuestion()
    {
        finalQuestionText.DOFade(0f, 0.2f);
    }

    public void RotateTimersContainer(float angle)
    {
        timersContainer.DOLocalRotate(new Vector3(0, angle, 0), 0.2f).SetUpdate(true);
    }

    public void StopGameTimer()
    {
        stopGameTimer.color = Color.green;
        stopGameTimer.fillAmount = 1f;
        stopGameTimer.DOFillAmount(0f, 7f).SetUpdate(true);
        stopGameTimer.DOColor(Color.yellow, 3.5f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
            stopGameTimer.DOColor(Color.red, 3.5f).SetEase(Ease.Linear).SetUpdate(true));
    }

    public IEnumerator FinishAnimation()
    {
        faceCloseup.gameObject.SetActive(true);
        yield return faceCloseup.Show();
        faceCloseup.gameObject.SetActive(false);

        yield return FinishCameraMovement();

        ScreenShatterManager shatter = Instantiate(screenShatter);
        yield return shatter.ScreenShatter();

        MusicManager.instance.StopSong();
    }

    private IEnumerator FinishCameraMovement()
    {
        CameraController.instance.cameraTransform.localPosition = new Vector3(0, 4.3f, -5.4f);
        CameraController.instance.cameraTransform.localRotation = Quaternion.Euler(0, 0, 20);

        yield return new WaitForSeconds(0.5f);

        CameraController.instance.cameraTransform.localPosition = new Vector3(0, 4.3f, -7f);
        CameraController.instance.cameraTransform.localRotation = Quaternion.Euler(0, 0, -20);

        yield return new WaitForSeconds(0.5f);

        CameraController.instance.cameraTransform.localPosition = new Vector3(0, 4.3f, -9f);
        CameraController.instance.cameraTransform.localRotation = Quaternion.Euler(0, 0, 10);
    }
}