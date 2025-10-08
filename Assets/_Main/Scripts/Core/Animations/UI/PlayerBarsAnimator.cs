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
    private CanvasGroup globalHealthBarCanvasGroup;

    public Image globalConcentrationBar;

    public RectTransform debateHealthContainer;
    public Image debateHealthMeter;
    private CanvasGroup debateHealthBarCanvasGroup;

    public Image debateConcentrationBar;

    public AudioClip damageSound;

    void Start()
    {
        globalHealthBarCanvasGroup = globalHealthContainer.GetComponent<CanvasGroup>();
        debateHealthBarCanvasGroup = debateHealthContainer.GetComponent<CanvasGroup>();
    }
    

    public void IncreaseHealth(float amount, float duration)
    {
        globalHealthMeter.DOKill();
        debateHealthMeter.DOKill();

        float newFillAmount = globalHealthMeter.fillAmount + amount / fullHpImageDivideAmount;

        globalHealthMeter.DOColor(Color.green, duration).OnComplete(() => globalHealthMeter.DOColor(Color.white, duration));
        debateHealthMeter.DOColor(Color.green, duration).OnComplete(() => debateHealthMeter.DOColor(Color.white, duration));
        
        ChangeFillAmount(newFillAmount, duration);
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
        
        ChangeFillAmount(newFillAmount, duration);
    }
    
    public void UpdateHealthBars(float newAmount)
    {
        float newFillAmount = newAmount / fullHpImageDivideAmount;

        globalHealthMeter.fillAmount = newFillAmount;
        debateHealthMeter.fillAmount = newFillAmount;
    }

    void ChangeFillAmount(float fillAmount, float duration)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(globalHealthMeter.DOFillAmount(fillAmount, duration));
        seq.Join(debateHealthMeter.DOFillAmount(fillAmount, duration));

        seq.Join(globalHealthContainer.DOScale(1.5f, duration));
        seq.Join(debateHealthContainer.DOScale(1.5f, duration));
        
        seq.Append(globalHealthContainer.DOScale(1f, duration));
        seq.Append(debateHealthContainer.DOScale(1f, duration));
    }

    public void ShowDebateBars(float duration)
    {
        debateHealthBarCanvasGroup.DOFade(1f, duration);
    }
    public void HideDebateBars(float duration)
    {
        debateHealthBarCanvasGroup.DOFade(0f, duration);
    }
    
    public void ShowGlobalBars(float duration)
    {
        globalHealthBarCanvasGroup.DOFade(1f, duration);
    }

    public void HideGlobalBars(float duration)
    {
        globalHealthBarCanvasGroup.DOFade(0f, duration);
    }
}
