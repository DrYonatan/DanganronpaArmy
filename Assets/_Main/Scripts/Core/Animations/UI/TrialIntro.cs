using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrialIntro : MonoBehaviour
{
    public List<Image> letters;
    public Image allRise;
    public Image stains;
    public TextMeshProUGUI classTrial;
    public AudioClip letterSound;
    public AudioClip allRiseSound;

    public IEnumerator Animate()
    {
        Initialize();
        
        StartCoroutine(LettersAnimation());
        
        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(0.3f);
        sequence.Append(stains.DOFade(1f, 0f));
        sequence.Append(stains.rectTransform.DOScale(Vector3.one, 0.1f));
        sequence.AppendInterval(0.2f);
        sequence.Append(classTrial.DOFade(1f, 0.2f));
        sequence.AppendInterval(1f);

        yield return new WaitForSeconds(2f);
        
        ImageScript.instance.FadeToBlack(0.1f);
        foreach (Image letter in letters)
        {
            LetterDisappear(letter);
            yield return new WaitForSeconds(0.01f);
        }
        
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    private void Initialize()
    {
        stains.DOFade(0f, 0f);
        stains.rectTransform.localScale = Vector3.one * 1.5f;
        allRise.DOFade(0f, 0f);
        classTrial.DOFade(0f, 0f);

        foreach (Image letter in letters)
        {
            Color color = letter.color;
            color.a = 0f;
            letter.color = color;
        }
    }

    private IEnumerator LettersAnimation()
    {
        foreach (Image letter in letters)
        {
            SoundManager.instance.PlaySoundEffect(letterSound);
            yield return LetterAppear(letter);
        }
        
        yield return new WaitForSeconds(0.2f);
       
        SoundManager.instance.PlaySoundEffect(allRiseSound);
        yield return LetterAppear(allRise);
    }

    private IEnumerator LetterAppear(Image letter)
    {
        Vector3 originalRotation = letter.rectTransform.eulerAngles;
        letter.rectTransform.localScale *= 2f;
        letter.rectTransform.localRotation = Quaternion.Euler(0f, 0f, originalRotation.z + 180f);
        letter.rectTransform.DOLocalRotate(originalRotation, 0.2f);
        letter.DOFade(1f, 0.2f);
        letter.rectTransform.DOScale(letter.rectTransform.localScale / 2f, 0.2f);
        
        yield return new WaitForSeconds(0.2f);
        
        float originalY = letter.rectTransform.anchoredPosition.y;
        letter.rectTransform.DOAnchorPosY(originalY - 30f, 0.05f).SetLoops(4, LoopType.Yoyo);


    }

    private void LetterDisappear(Image letter)
    {
        Vector3 originalRotation = letter.rectTransform.eulerAngles;
        letter.rectTransform.DOLocalRotate(originalRotation + new Vector3(0, 0, 180), 0.1f);
        letter.DOFade(0f, 0.1f);
        letter.rectTransform.DOScale(letter.rectTransform.localScale * 2f, 0.1f);
    }

}
