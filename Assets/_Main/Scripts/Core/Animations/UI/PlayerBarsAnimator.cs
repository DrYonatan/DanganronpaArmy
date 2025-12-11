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
    }

    public void IncreaseHealth(float amount, float duration)
    {
        globalHealthMeter.DOKill();
        debateHealthMeter.DOKill();

        float newFillAmount = globalHealthMeter.fillAmount + amount / fullHpImageDivideAmount;

        GlowMeter(globalHealthMeter, Color.green, duration);
        GlowMeter(debateHealthMeter, Color.green, duration);

        ChangeHealthFillAmount(newFillAmount, duration);
    }

    private void GlowMeter(Image meter, Color color, float duration)
    {
        meter.DOColor(color, 0.05f).OnComplete(() => meter.DOColor(Color.white, 0.05f).SetDelay(duration));
    }

    public void DecreaseHealth(float amount, float duration)
    {
        globalHealthMeter.DOKill();
        debateHealthMeter.DOKill();
        
        BlinkMeter(globalHealthMeter, Color.red, 6);
        BlinkMeter(debateHealthMeter, Color.red, 6);

        SoundManager.instance.PlaySoundEffect(damageSound);

        ChangeHealthFillAmount(amount / fullHpImageDivideAmount, duration);
    }

    public void DecreaseHealthFromMeter(Image meter, float newAmount, float duration)
    {
        meter.DOKill();
        BlinkMeter(meter, Color.red, 6);
        meter.color = Color.white;
        float newFillAmount = newAmount / 10;

        meter.DOFillAmount(newFillAmount, duration);
    }

    private void StopCurrentConcentrationBarsAnimation()
    {
        globalConcentrationMeter.color = Color.white;
        debateConcentrationMeter.color = Color.white;
        globalConcentrationMeter.DOKill();
        debateConcentrationMeter.DOKill();
    }

    private void BlinkMeter(Image meter, Color color, int repeatAmount)
    {
        meter.DOColor(color, 0.05f).SetUpdate(true)
            .SetLoops(repeatAmount, LoopType.Yoyo).OnComplete(() => globalHealthMeter.color = Color.white);
    }

    public void UpdateConcentration(float newConcentration)
    {
        globalConcentrationMeter.fillAmount = newConcentration / fullConcentrationImageDivideAmount;
        debateConcentrationMeter.fillAmount = newConcentration / fullConcentrationImageDivideAmount;
    }

    public void FillConcentrationEffect(float duration)
    {
        StopCurrentConcentrationBarsAnimation();
        GlowMeter(globalConcentrationMeter, Color.green, duration);
        GlowMeter(debateConcentrationMeter, Color.green, duration);
    }

    public void DrainConcentrationEffect()
    {
        StopCurrentConcentrationBarsAnimation();

        BlinkMeter(globalConcentrationMeter, Color.blue, -1);
        BlinkMeter(debateConcentrationMeter, Color.blue, -1);
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
        seq.Join(globalConcentrationContainer.DOAnchorPosY(
            originalGlobalConcentrationY - globalHealthMeter.GetComponent<RectTransform>().rect.height * 0.5f,
            duration));
        seq.Join(debateConcentrationContainer.DOAnchorPosY(
            originalDebateConcentrationY - debateHealthMeter.GetComponent<RectTransform>().rect.height * 0.5f,
            duration));

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