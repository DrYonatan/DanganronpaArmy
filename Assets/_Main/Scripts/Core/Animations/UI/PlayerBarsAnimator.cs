using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBarsAnimator : MonoBehaviour
{
    public float fullHpImageDivideAmount;
    public float fullConcentrationImageDivideAmount;
    
    public RectTransform globalHealthContainer;
    public Image globalHealthMeter;

    public RectTransform globalConcentrationContainer;
    public Image globalConcentrationMeter;
    
    public CanvasGroup globalBarsContainer;

    public RectTransform debateHealthContainer;
    public Image debateHealthMeter;

    public RectTransform debateConcentrationContainer;
    public Image debateConcentrationMeter;
    
    public CanvasGroup debateBarsContainer;
    
    public AudioClip damageSound;

    void Awake()
    {
        SetBarsFillAmount(TrialManager.instance.playerStats.hp, TrialManager.instance.playerStats.concentration);
    }

    public void IncreaseHealth(float amount, float duration)
    {
        globalHealthMeter.DOKill();
        debateHealthMeter.DOKill();

        float newFillAmount = globalHealthMeter.fillAmount + amount / fullHpImageDivideAmount;

        globalHealthMeter.DOColor(Color.green, duration).OnComplete(() => globalHealthMeter.DOColor(Color.white, duration));
        debateHealthMeter.DOColor(Color.green, duration).OnComplete(() => debateHealthMeter.DOColor(Color.white, duration));
        
        ChangeHealthFillAmount(newFillAmount, duration);
    }

    public void DecreaseHealth(float amount, float duration)
    {
        globalHealthMeter.DOKill();
        debateHealthMeter.DOKill();

        float newFillAmount = globalHealthMeter.fillAmount - amount / fullHpImageDivideAmount;
        
        globalHealthMeter.DOColor(Color.red, 0.05f)
            .SetLoops(6, LoopType.Yoyo).OnComplete(() => globalHealthMeter.color = Color.white);
        debateHealthMeter.DOColor(Color.red, 0.05f)
            .SetLoops(6, LoopType.Yoyo).OnComplete(() => globalHealthMeter.color = Color.white);;
        
        SoundManager.instance.PlaySoundEffect(damageSound);
        
        ChangeHealthFillAmount(newFillAmount, duration);
    }

    private void StopCurrentBarsAnimation()
    {
        globalConcentrationMeter.DOKill();
        debateConcentrationMeter.DOKill();
    }

    public void FillConcentration(float duration)
    {
        StopCurrentBarsAnimation();
        globalConcentrationMeter.DOFillAmount(1f, duration).SetEase(Ease.Linear).SetUpdate(true);
        debateConcentrationMeter.DOFillAmount(1f, duration).SetEase(Ease.Linear).SetUpdate(true);
    }

    public void DrainConcentration(float duration)
    {
        StopCurrentBarsAnimation();
        globalConcentrationMeter.DOFillAmount(0f, duration).SetEase(Ease.Linear);
        debateConcentrationMeter.DOFillAmount(0f, duration).SetEase(Ease.Linear);
    }
    
    public void SetBarsFillAmount(float healthFillAmount, float concentrationFillAmount)
    {
        float newHealthFillAmount = healthFillAmount / fullHpImageDivideAmount;
        float newConcentrationFillAmount = concentrationFillAmount / fullConcentrationImageDivideAmount;

        globalHealthMeter.fillAmount = newHealthFillAmount;
        debateHealthMeter.fillAmount = newHealthFillAmount;

        globalConcentrationMeter.fillAmount = newConcentrationFillAmount;
        debateConcentrationMeter.fillAmount = newConcentrationFillAmount;
    }
    

    void ChangeHealthFillAmount(float fillAmount, float duration)
    {
        float originalGlobalConcentrationY = globalConcentrationContainer.anchoredPosition.y;
        float originalDebateConcentrationY = debateConcentrationContainer.anchoredPosition.y;
        
        Sequence seq = DOTween.Sequence();

        seq.Append(globalHealthMeter.DOFillAmount(fillAmount, duration));
        seq.Join(debateHealthMeter.DOFillAmount(fillAmount, duration));

        seq.Join(globalHealthContainer.DOScale(1.5f, duration));
        seq.Join(debateHealthContainer.DOScale(1.5f, duration));
        seq.Join(globalConcentrationContainer.DOAnchorPosY(originalGlobalConcentrationY - globalHealthMeter.GetComponent<RectTransform>().rect.height * 0.5f, duration));
        seq.Join(debateConcentrationContainer.DOAnchorPosY(originalDebateConcentrationY - debateHealthMeter.GetComponent<RectTransform>().rect.height * 0.5f, duration));
        
        seq.Append(globalHealthContainer.DOScale(1f, duration));
        seq.Join(debateHealthContainer.DOScale(1f, duration));
        seq.Join(globalConcentrationContainer.DOAnchorPosY(originalGlobalConcentrationY, duration));
        seq.Join(debateConcentrationContainer.DOAnchorPosY(originalDebateConcentrationY, duration));
    }

    public void ShowDebateBars(float duration)
    {
        debateBarsContainer.DOFade(1f, duration);
    }
    public void HideDebateBars(float duration)
    {
        debateBarsContainer.DOFade(0f, duration);
    }
    
    public void ShowGlobalBars(float duration)
    {
        globalBarsContainer.DOFade(1f, duration);
    }

    public void HideGlobalBars(float duration)
    {
        globalBarsContainer.DOFade(0f, duration);
    }
}
