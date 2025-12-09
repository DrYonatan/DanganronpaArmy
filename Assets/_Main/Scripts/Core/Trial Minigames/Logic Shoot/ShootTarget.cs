using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ShootTarget : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public List<ShootTargetArea> areas;
    public float timeOut;
    public float movementTime;

    public Vector2 targetPosition;

    private bool isDisappearing;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void LifeTime()
    {
        StartCoroutine(LifeTimeRoutine());
    }

    private IEnumerator LifeTimeRoutine()
    {
        rectTransform.DOAnchorPos(targetPosition, movementTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        yield return new WaitForSeconds(timeOut);
        if (!isDisappearing)
        {
            rectTransform.DOKill();
            LogicShootManager.instance.DamagePlayer(1f);
            Destroy(gameObject);
        }
    }

    public void DisappearAnimation()
    {
        rectTransform.DOKill();
        isDisappearing = true;
        rectTransform.DOAnchorPosY(-1000, 0.5f).OnComplete(() => Destroy(gameObject));
    }
}