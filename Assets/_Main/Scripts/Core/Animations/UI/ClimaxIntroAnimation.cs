using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ClimaxIntroAnimation : MonoBehaviour
{
    public Image frame1;
    public Image frame2;
    public Image frame3;

    public Image explosion;

    public Image closing;
    public Image argument;
    public Image climaxInterface;

    public Image closingArgumentBlur;
    public Image closingArgumentGlow;

    public Image begin;
    public Image beginBlur;
    public Image beginGlow;

    public Image start;

    public Image lines;

    public AudioClip soundEffect;

    public IEnumerator PlayAnimation()
    {
        Initialize();
        
        SoundManager.instance.PlaySoundEffect(soundEffect);
        float framesDuration = 0.1f;
        Sequence seq =  DOTween.Sequence();
        seq.Append(frame1.DOFade(0f, framesDuration));
        seq.Append(frame2.DOFade(0f, framesDuration));

        explosion.rectTransform.DOScale(Vector3.one,  framesDuration * 6f);
        
        yield return new WaitForSeconds(framesDuration * 3f);

        RotatingTextAppear(closing);
        
        yield return new WaitForSeconds(0.3f);
        
        RotatingTextAppear(argument);
        climaxInterface.DOFade(1f, 0.2f);

        yield return new WaitForSeconds(0.2f);
        
        yield return BlurGlowText(closingArgumentBlur, closingArgumentGlow);
        explosion.DOFade(0f, 0.2f);
        
        yield return new WaitForSeconds(0.3f);

        begin.DOFade(1f, 0.1f);
        begin.rectTransform.DOScale(1f, 0.1f);

        yield return new WaitForSeconds(0.1f);

        climaxInterface.DOFade(0f, 0.2f);
        start.DOFade(1f, 0.2f);

        closingArgumentGlow.DOFade(0f, 0.2f);
        closing.DOFade(0f, 0.2f);
        argument.DOFade(0f, 0.2f);
        Shake(frame3.rectTransform, -1f);
        Shake(begin.rectTransform, 1f);
        
        yield return new WaitForSeconds(0.1f);
        
        lines.DOFade(1f, 0.1f);

        yield return new WaitForSeconds(0.1f);

        yield return BlurGlowText(begin, beginGlow);

        frame3.DOFade(0f, 0.1f);
        frame3.rectTransform.DOScale(Vector3.one * 3f, 0.2f);

        yield return new WaitForSeconds(0.2f);
        
        beginGlow.DOFade(0f, 0.1f);
        beginGlow.rectTransform.DOScale(Vector3.one * 3f, 0.2f);

        start.DOFade(0f, 0.4f);
        lines.DOFade(0f, 0.4f);

        yield return new WaitForSeconds(0.4f);
    }

    private void Initialize()
    {
        explosion.rectTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        
        Color color = Color.white;
        color.a = 0f;
        
        closing.color = color;
        
        closing.rectTransform.localRotation = Quaternion.Euler(0f, 0f, 180f);
        closing.rectTransform.localScale = Vector3.one * 2f;
        
        argument.color = color;
        
        argument.rectTransform.localRotation = Quaternion.Euler(0f, 0f, 180f);
        argument.rectTransform.localScale = Vector3.one * 2f;

        climaxInterface.color = color;
        
        closingArgumentBlur.color = color;
        closingArgumentBlur.rectTransform.localScale = Vector3.one;

        closingArgumentGlow.color = color;

        begin.color = color;
        beginBlur.color = color;
        beginGlow.color = color;
        
        beginBlur.rectTransform.localScale = Vector3.one;
        begin.rectTransform.localScale = Vector3.one * 1.4f;

        start.color = color;

        lines.color = color;
    }

    private void RotatingTextAppear(Image text)
    {
        text.DOFade(1f,  0.1f);
        text.rectTransform.DOLocalRotate(Vector3.zero, 0.15f);
        text.rectTransform.DOScale(Vector3.one, 0.15f);
    }

    private void Shake(RectTransform rect, float direction)
    {
        float originalY = rect.localPosition.y;
        rect.DOAnchorPosY(originalY + 15f * direction, 0.05f).SetLoops(8, LoopType.Yoyo);
    }

    private IEnumerator BlurGlowText(Image blur, Image glow)
    {
        blur.DOFade(1f, 0.1f);
        blur.rectTransform.DOScale(Vector3.one * 0.9f, 0.15f);
        
        yield return new WaitForSeconds(0.15f);
        
        blur.rectTransform.DOScale(Vector3.one * 1.5f, 0.4f);
        blur.DOFade(0f, 0.4f);
        glow.DOFade(1f, 0.2f);
    }
}
