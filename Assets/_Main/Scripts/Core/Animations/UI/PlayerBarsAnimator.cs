using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBarsAnimator : MonoBehaviour
{
    public Image globalHealthBar;

    public Image globalConcentrationBar;

    public Image debateHealthBar;

    public Image debateConcentrationBar;

    public void IncreaseHealth(float amount)
    {
        globalHealthBar.DOKill();
        debateHealthBar.DOKill();
        
        float newFillAmount = globalHealthBar.fillAmount + amount / TrialManager.instance.playerStats.maxHP;

        ChangeFillAmount(newFillAmount);
    }
    
    public void DecreaseHealth(float amount)
    {
        globalHealthBar.DOKill();
        debateHealthBar.DOKill();
        
        float newFillAmount = globalHealthBar.fillAmount - amount / TrialManager.instance.playerStats.maxHP;

        ChangeFillAmount(newFillAmount);
    }

    void ChangeFillAmount(float fillAmount)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(globalHealthBar.DOFillAmount(fillAmount, 0.5f));
        seq.Join(debateHealthBar.DOFillAmount(fillAmount, 0.5f));

        seq.Join(globalHealthBar.transform.DOScale(1.5f, 0.2f));
        seq.Join(debateHealthBar.transform.DOScale(1.5f, 0.2f));
        
        seq.Append(globalHealthBar.transform.DOScale(1f, 0.2f));
        seq.Append(debateHealthBar.transform.DOScale(1f, 0.2f));
    }

    public void ShowDebateBars(float duration)
    {
        debateHealthBar.DOFade(1f, duration);
    }
    public void HideDebateBars(float duration)
    {
        debateHealthBar.DOFade(0f, duration);
    }
    
    public void ShowGlobalBars(float duration)
    {
        globalHealthBar.DOFade(1f, duration);
    }

    public void HideGlobalBars(float duration)
    {
        globalHealthBar.DOFade(0f, duration);
    }
}
