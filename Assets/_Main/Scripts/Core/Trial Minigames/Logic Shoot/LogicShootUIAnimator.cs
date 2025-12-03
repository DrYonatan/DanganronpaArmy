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

    public Image enemyHpBar;
    public List<ShootTarget> targets;
    public RectTransform targetsContainer;

    public void ShowMikbazText(int number)
    {
        TextMeshProUGUI mikbazText = Instantiate(mikbazTextPrefab, transform);
        mikbazText.text = "מקבץ " + number + " !!";

        Sequence seq = DOTween.Sequence();
        seq.Append(mikbazText.DOFade(0f, 0.05f).SetLoops(6, LoopType.Yoyo));
        seq.Append(mikbazText.rectTransform.DOAnchorPos(enemyHpBar.rectTransform.anchoredPosition, 1f)
            .SetEase(Ease.InCubic));
        seq.Join(mikbazText.rectTransform.DOScale(0.1f, 1f).SetEase(Ease.InCubic));

        seq.OnComplete(() => Destroy(mikbazText.gameObject));
    }

    public void GenerateTargets(List<ShootTargetData> shootTargets)
    {
        foreach (ShootTargetData target in shootTargets)
        {
            ShootTarget newTarget = Instantiate(targetPrefab, targetsContainer);
            for (int i = 0; i < newTarget.areas.Count; i++)
            {
                newTarget.questionText.text = target.question;
                newTarget.areas[i].isCorrect = target.answers[i].isCorrect;
                newTarget.areas[i].answer.text = target.answers[i].answer;
                newTarget.timeOut =  target.timeOut;
                StartCoroutine(newTarget.LifeTime());
            }
        }
    }
}