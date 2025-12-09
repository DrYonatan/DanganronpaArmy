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

    public AudioClip music;

    public Image playerHpBar;
    public Image enemyHpBar;
    
    public RectTransform targetsContainer;
    public TextMeshProUGUI ammoNumberText;

    public MinigameStartAnimation startAnimation;

    public void ShowMikbazText(int number)
    {
        TextMeshProUGUI mikbazText = Instantiate(mikbazTextPrefab, transform);
        mikbazText.text = "מקבץ " + number + " !!";

        Sequence seq = DOTween.Sequence();
        seq.Append(mikbazText.DOFade(0f, 0.05f).SetLoops(6, LoopType.Yoyo));
        seq.Append(mikbazText.rectTransform.DOMove(enemyHpBar.rectTransform.position, 1f)
            .SetEase(Ease.InCubic));
        seq.Join(mikbazText.rectTransform.DOScale(0.1f, 1f).SetEase(Ease.InCubic));

        seq.OnComplete(() => OnMikbazFinish(mikbazText.gameObject, (10f - number) / 10f));
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
                newTarget.movementTime =  target.movementTime;
                newTarget.LifeTime();
            }
        }

        yield return new WaitForSeconds(duration);
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

}