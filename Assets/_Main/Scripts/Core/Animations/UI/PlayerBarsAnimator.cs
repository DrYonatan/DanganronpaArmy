using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBarsAnimator : MonoBehaviour
{
    public float fullHpImageDivideAmount;
    public float fullConcentrationImageDivideAmount;
    
    public Image globalHealthBar;
    public Image globalHealthBackground;

    public Image globalConcentrationBar;
    public Image globalConcentrationBackground;

    public Image debateHealthBar;
    public Image debateHealthBackground;

    public Image debateConcentrationBar;
    public Image debateConcentrationBackground;

    public AudioClip damageSound;

    public void IncreaseHealth(float amount, float duration)
    {
        globalHealthBar.DOKill();
        debateHealthBar.DOKill();

        float newFillAmount = globalHealthBar.fillAmount + amount / fullHpImageDivideAmount;

        globalHealthBar.DOColor(Color.green, duration).OnComplete(() => globalHealthBar.DOColor(Color.white, duration));
        debateHealthBar.DOColor(Color.green, duration).OnComplete(() => debateHealthBar.DOColor(Color.white, duration));
        
        ChangeFillAmount(newFillAmount, duration);
    }

    public void DecreaseHealth(float amount, float duration)
    {
        globalHealthBar.DOKill();
        debateHealthBar.DOKill();

        float newFillAmount = globalHealthBar.fillAmount - amount / fullHpImageDivideAmount;
        
        globalHealthBar.DOColor(Color.red, 0.05f)
            .SetLoops(6, LoopType.Yoyo).OnComplete(() => globalHealthBar.color = Color.white);
        debateHealthBar.DOColor(Color.red, 0.05f)
            .SetLoops(6, LoopType.Yoyo).OnComplete(() => globalHealthBar.color = Color.white);;
        
        SoundManager.instance.PlaySoundEffect(damageSound);
        
        ChangeFillAmount(newFillAmount, duration);
    }
    
    public void UpdateHealthBars(float newAmount)
    {
        float newFillAmount = newAmount / fullHpImageDivideAmount;

        globalHealthBar.fillAmount = newFillAmount;
        debateHealthBar.fillAmount = newFillAmount;
    }

    void ChangeFillAmount(float fillAmount, float duration)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(globalHealthBar.DOFillAmount(fillAmount, duration));
        seq.Join(debateHealthBar.DOFillAmount(fillAmount, duration));

        seq.Join(globalHealthBar.transform.DOScale(1.5f, duration));
        seq.Join(globalHealthBackground.transform.DOScale(1.5f, duration));
        seq.Join(debateHealthBar.transform.DOScale(1.5f, duration));
        seq.Join(debateHealthBackground.transform.DOScale(1.5f, duration));
        
        seq.Append(globalHealthBar.transform.DOScale(1f, duration));
        seq.Join(globalHealthBackground.transform.DOScale(1f, duration));
        seq.Append(debateHealthBar.transform.DOScale(1f, duration));
        seq.Join(debateHealthBackground.transform.DOScale(1f, duration));
    }

    public void ShowDebateBars(float duration)
    {
        debateHealthBar.DOFade(1f, duration);
        debateHealthBackground.DOFade(1f, duration);
    }
    public void HideDebateBars(float duration)
    {
        debateHealthBar.DOFade(0f, duration);
        debateHealthBackground.DOFade(0f, duration);
    }
    
    public void ShowGlobalBars(float duration)
    {
        globalHealthBar.DOFade(1f, duration);
        globalHealthBackground.DOFade(1f, duration);
    }

    public void HideGlobalBars(float duration)
    {
        globalHealthBar.DOFade(0f, duration);
        globalHealthBackground.DOFade(0f, duration);
    }
}
