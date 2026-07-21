using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SectionStartEffect : MonoBehaviour
{
    public Image background;
    public CanvasGroup backgroundContainer;
    public TextMeshProUGUI text;
    public AudioClip soundEffect;

    public void Animate(string sectionTitle, float delay = 0f)
    {
        Initialize(sectionTitle);

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(delay);
        sequence.AppendCallback(() => SoundManager.instance.PlaySoundEffect(soundEffect));

        sequence.Append(backgroundContainer.transform.DOScaleX(1f, 0.1f).From(0f));

        sequence.AppendInterval(0.05f);

        sequence.Append(backgroundContainer.transform.DOScaleX(1.5f, 0.8f).SetEase(Ease.Linear));

        sequence.Join(text.DOFade(1f, 0.4f).SetEase(Ease.OutQuad));

        sequence.AppendInterval(0.3f);

        sequence.Append(transform.DOScale(Vector3.one * 1.5f, 0.3f));
        sequence.Join(text.DOFade(0f, 0.3f));
        sequence.Join(backgroundContainer.DOFade(0f, 0.3f));
    }

    private void Initialize(string sectionTitle)
    {
        text.text = sectionTitle;
        text.alpha = 0f;
        backgroundContainer.alpha = 1f;

        backgroundContainer.transform.localScale = new Vector3(0f, 1f, 1f);

        transform.localScale = Vector3.one;
    }
}
