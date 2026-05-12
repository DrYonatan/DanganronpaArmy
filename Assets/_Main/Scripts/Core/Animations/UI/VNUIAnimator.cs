using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VNUIAnimator : MonoBehaviour
{
    public static VNUIAnimator instance { get; private set; }
    public RectTransform mainContainer;
    public Image timeContainer;
    public Image timeIcon;
    public TextMeshProUGUI chapterNameText;
    public TextMeshProUGUI timeText;

    public RadioBoxAnimator musicBoxContainer;
    public TextMeshProUGUI musicName;

    private Vector2 mainContainerOriginalPos;
    private Vector2 timeContainerOriginalPos;


    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            mainContainerOriginalPos = mainContainer.anchoredPosition;
            timeContainerOriginalPos = timeContainer.rectTransform.anchoredPosition;
        }
    }

    public void Appear(float duration = 0.5f)
    {
        Sequence seq = DOTween.Sequence();

        mainContainer.anchoredPosition = mainContainerOriginalPos - new Vector2(600, 0);
        timeContainer.rectTransform.anchoredPosition = timeContainerOriginalPos + new Vector2(0, 80);

        seq.Append(mainContainer.DOAnchorPosX(mainContainerOriginalPos.x, duration));
        seq.Append(timeContainer.rectTransform.DOAnchorPosY(timeContainerOriginalPos.y, duration));
        seq.Append(musicBoxContainer.rectTransform.DOAnchorPosX(0, duration / 2));

        musicName.rectTransform.anchoredPosition = new Vector2(-400, 0);
        musicName.rectTransform.DOAnchorPosX(450, 5f).SetEase(Ease.Linear).SetLoops(-1).SetLink(musicName.gameObject);
    }

    public void Disappear(float duration = 0.3f)
    {
        Sequence seq = DOTween.Sequence();

        mainContainer.anchoredPosition = mainContainerOriginalPos;
        timeContainer.rectTransform.anchoredPosition = timeContainerOriginalPos;

        seq.Append(timeContainer.rectTransform.DOAnchorPosY(timeContainerOriginalPos.y + 80, duration));
        seq.Append(mainContainer.DOAnchorPosX(mainContainerOriginalPos.x - 600, duration));
        seq.Append(musicBoxContainer.rectTransform.DOAnchorPosX(500, duration / 2));
        seq.AppendCallback(() => musicName.rectTransform.DOKill());
    }
}