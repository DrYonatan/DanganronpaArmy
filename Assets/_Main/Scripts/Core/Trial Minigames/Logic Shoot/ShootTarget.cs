using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShootTarget : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public List<ShootTargetArea> areas;
    public float timeOut;
    public float movementTime;
    public CanvasGroup canvasGroup;
    public Image lines;
    public Image bubble;
    public AudioClip completeSound;

    public Vector2 targetPosition;

    private bool isDisappearing;

    private RectTransform rectTransform;

    private float elapsedTime;
    private float phase;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (isDisappearing)
            return;

        float normalizedTime = Mathf.Clamp01(elapsedTime / timeOut);

        float frequency = Mathf.Lerp(
            1f,
            8f,
            Mathf.SmoothStep(0f, 1f, normalizedTime)
        );

        phase += Time.deltaTime * frequency;

        float pingPong = Mathf.PingPong(phase, 1f);
        float scale = Mathf.Lerp(1f, 1.2f, pingPong);

        bubble.rectTransform.localScale = Vector3.one * scale;
    }

    public void LifeTime()
    {
        StartCoroutine(LifeTimeRoutine());
    }

    private IEnumerator LifeTimeRoutine()
    {
        rectTransform.DOAnchorPos(targetPosition, movementTime).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject)
            .SetEase(Ease.Linear);
        bubble.color = Color.green;
        bubble.DOColor(Color.yellow, timeOut / 2).SetEase(Ease.Linear).SetLink(gameObject).OnComplete(() =>
            bubble.DOColor(Color.red, timeOut / 2).SetEase(Ease.Linear).SetLink(gameObject));

        lines.color = Color.green;
        lines.DOColor(Color.yellow, timeOut / 2).SetEase(Ease.Linear).SetLink(gameObject).OnComplete(() =>
            lines.DOColor(Color.red, timeOut / 2).SetEase(Ease.Linear).SetLink(gameObject));

        StartCoroutine(CountTime());
        yield return new WaitForSeconds(timeOut);

        if (!isDisappearing && LogicShootManager.instance.isActive)
        {
            rectTransform.DOKill();
            LogicShootManager.instance.DamagePlayer(1f);
            LogicShootManager.instance.animator.TargetFailExplosion(rectTransform.anchoredPosition);
            canvasGroup.DOFade(0f, 0.5f).OnComplete(() => Destroy(gameObject)).SetLink(gameObject);
        }
    }

    private IEnumerator CountTime()
    {
        while (elapsedTime < timeOut)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void DisappearAnimation()
    {
        bubble.DOFade(0f, 0.1f).SetLink(gameObject);
        if(rectTransform != null)
           rectTransform.DOKill();
        rectTransform.DOAnchorPosY(-1000, 0.5f).OnComplete(() => Destroy(gameObject)).SetLink(gameObject);
    }

    public void SetIsDisappearing()
    {
        isDisappearing = true;
    }
}