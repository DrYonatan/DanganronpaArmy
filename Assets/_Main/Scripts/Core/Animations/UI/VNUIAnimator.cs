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

    private Vector2 mainContainerOriginalPos;
    private Vector2 timeContainerOriginalPos;

    void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            mainContainerOriginalPos =  mainContainer.anchoredPosition;
            timeContainerOriginalPos =  timeContainer.rectTransform.anchoredPosition;
        }
    }

    public void Appear()
    {
        Sequence seq = DOTween.Sequence();

        mainContainer.anchoredPosition = mainContainerOriginalPos - new Vector2(500, 0);
        timeContainer.rectTransform.anchoredPosition = timeContainerOriginalPos + new Vector2(0, 80);
        
        seq.Append(mainContainer.DOAnchorPosX(mainContainerOriginalPos.x, 0.5f));
        seq.Append(timeContainer.rectTransform.DOAnchorPosY(timeContainerOriginalPos.y, 0.4f));
    }

    public void Disappear()
    {
        Sequence seq = DOTween.Sequence();
        
        mainContainer.anchoredPosition = mainContainerOriginalPos;
        timeContainer.rectTransform.anchoredPosition = timeContainerOriginalPos;
        
        seq.Append(timeContainer.rectTransform.DOAnchorPosY(timeContainerOriginalPos.y + 80, 0.4f));
        seq.Append(mainContainer.DOAnchorPosX(mainContainerOriginalPos.x - 500, 0.5f));
    }
    
}
