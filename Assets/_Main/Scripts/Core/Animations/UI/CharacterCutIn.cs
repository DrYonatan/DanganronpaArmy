using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCutIn : MonoBehaviour
{
    public Image leftBar;
    public Image rightBar;

    public Image backgroundLines;
    public RectTransform characterBody;
    public RectTransform characterSubPart;
    public RectTransform text;

    public Sprite leftBarFirstFrame;
    public Sprite leftBarSecondFrame;

    public Sprite rightBarFirstFrame;
    public Sprite rightBarSecondFrame;

    private Vector2 initialBodyPosition;
    public Vector2 finalBodyPosition;
    public Vector2 finalSubPartPosition;

    public Vector3 finalBodyRotation;
    public Vector3 finalSubPartRotation;

    public Vector3 finalBodyScale;
    public Vector3 finalSubPartScale;

    public CanvasGroup canvasGroup;

    public AudioClip soundEffect;

    public float duration = 2f;

    public void Animate()
    {
        SetInitialValues();

        backgroundLines.rectTransform.DOScale(new Vector3(1.2f, 1f, 1f), 0.05f)
            .SetLoops(-1, LoopType.Yoyo);

        SoundManager.instance.PlaySoundEffect(soundEffect);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(1f, 0.2f));
        sequence.Join(characterBody.DOAnchorPos(initialBodyPosition, 0.2f));
        
        sequence.AppendCallback(() => BarsSpriteSwap());
        sequence.Append(characterBody.DOAnchorPos(finalBodyPosition, duration));
        sequence.Join(characterBody.DOLocalRotate(finalBodyRotation, duration));
        sequence.Join(characterBody.DOScale(finalBodyScale, duration));
        sequence.Join(characterSubPart.DOAnchorPos(finalSubPartPosition, duration));
        sequence.Join(characterSubPart.DOLocalRotate(finalSubPartRotation, duration));
        sequence.Join(characterSubPart.DOScale(finalSubPartScale, duration));
        sequence.Join(text.DOScale(1.2f, duration));

        sequence.Append(transform.DOScale(1.4f, 0.05f));
        sequence.Join(canvasGroup.DOFade(0f, 0.05f));

        sequence.OnComplete(() => Finish());
    }

    private void SetInitialValues()
    {
        initialBodyPosition = characterBody.anchoredPosition;
        characterBody.anchoredPosition -= new Vector2(60, 300);
        transform.localScale = new Vector3(0.3f, 1f, 1f);
    }

    private void Finish()
    {
        backgroundLines.rectTransform.DOKill();
        Destroy(gameObject);
    }

    private void BarsSpriteSwap()
    {
        float swapInterval = 0.1f;

        Sequence seq = DOTween.Sequence();

        seq.AppendCallback(() => leftBar.sprite = leftBarSecondFrame)
            .AppendInterval(swapInterval)
            .AppendCallback(() => leftBar.sprite = leftBarFirstFrame)
            .AppendInterval(swapInterval)
            .SetLoops(-1).SetTarget(backgroundLines.rectTransform);

        Sequence seq2 = DOTween.Sequence();

        seq2.AppendCallback(() => rightBar.sprite = rightBarSecondFrame)
            .AppendInterval(swapInterval)
            .AppendCallback(() => rightBar.sprite = rightBarFirstFrame)
            .AppendInterval(swapInterval)
            .SetLoops(-1).SetTarget(backgroundLines.rectTransform);
    }
}