using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class DebateUIAnimator : MonoBehaviour
{
    public RectTransform namePart;
    public RectTransform facePart;
    public RectTransform timePart;

    public RectTransform namePartOriginalPos;
    public RectTransform facePartOriginalPos;
    public RectTransform timePartOriginalPos;

    public CharacterFaceController characterFace;
    public GameObject nodeIndicatorPrefab;
    public Transform nodeIndicatorContainer;
    private List<Image> indicators = new List<Image>();
    public TextMeshProUGUI nameText;

    public RectTransform cylinder;
    public RectTransform bullet;
    public RectTransform bulletOriginalPos;

    public RectTransform circles;
    public DebateCircle smallCircle;
    public DebateCircle bigCircle;

    public BulletSelectionMenu bulletSelectionMenu;

    public float moveAmountY = 40f;
    public float moveAmountX = 150f;

    public float duration = 0.5f;
    public float reloadDuration = 0.2f;

    public void DebateUIAppear()
    {
        namePart.anchoredPosition = namePartOriginalPos.anchoredPosition + new Vector2(0, -moveAmountY);
        facePart.anchoredPosition = facePartOriginalPos.anchoredPosition + new Vector2(moveAmountX, 0);
        timePart.anchoredPosition = timePartOriginalPos.anchoredPosition + new Vector2(0, moveAmountY);

        namePart.DOAnchorPos(namePartOriginalPos.anchoredPosition, duration).SetEase(Ease.OutQuad);
        facePart.DOAnchorPos(facePartOriginalPos.anchoredPosition, duration).SetEase(Ease.OutQuad);
        timePart.DOAnchorPos(timePartOriginalPos.anchoredPosition, duration).SetEase(Ease.OutQuad);

        CursorManager.instance.Show();
        GenerateIndicators();
    }
    
    public void LoadBullets(List<Evidence> evidences)
    {
        bulletSelectionMenu.LoadBullets(evidences);
    }
    public void OpenBulletSelectionMenu()
    {
        if (!bulletSelectionMenu.isOpen)
        {
            bulletSelectionMenu.Appear();
            bulletSelectionMenu.OpenBullets();
            HideCylinderAndCircles();
        }
        
    }

    public void CloseBulletSelectionMenu()
    {
        if (bulletSelectionMenu.isOpen)
        {
            bulletSelectionMenu.Disappear();
            ShowCylinderAndCircles(); 
        }
        
    }
    public void DebateUIDisappear()
    {
        namePart.anchoredPosition = namePartOriginalPos.anchoredPosition + new Vector2(0, -moveAmountY);
        facePart.anchoredPosition = facePartOriginalPos.anchoredPosition + new Vector2(moveAmountX, 0);
        timePart.anchoredPosition = timePartOriginalPos.anchoredPosition + new Vector2(0, moveAmountY);
        HideCylinderAndCircles();
        CursorManager.instance.Hide();
    }

    public void ChangeFace(string characterName)
    {
        characterFace.SetFace(characterName);
    }
    
    void GenerateIndicators()
    {
        // Clear old indicators
        foreach (Transform child in nodeIndicatorContainer)
            Destroy(child.gameObject);

        indicators.Clear();

        for (int i = 0; i < GameLoop.instance.stage.dialogueNodes.Count; i++)
        {
            GameObject indicator = Instantiate(nodeIndicatorPrefab, nodeIndicatorContainer);
            Image image = indicator.GetComponent<Image>();
            image.color = Color.gray;
            indicators.Add(image);
        }
    }
    public void HighlightNode(int index)
    {
        for (int i = 0; i < indicators.Count; i++)
        {
            indicators[i].color = Color.gray;
            indicators[i].DOComplete();  // Stop any running tweens
            indicators[i].DOKill();
        }
        indicators[index].DOColor(new Color(1f, 1f, 0.5f), 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    public void UpdateName(string characterName)
    {
        nameText.text = characterName;
    }
    
    void UnLoadBullet()
    {
        bullet.DOAnchorPosX(bulletOriginalPos.anchoredPosition.x - 300f, reloadDuration).SetEase(Ease.OutQuad);
    }

    public void LoadBullet()
    {
        if (!bulletSelectionMenu.isOpen)
        {
            bullet.GetComponent<UIBullet>().UnWhitenBullet();
            bullet.anchoredPosition = bulletOriginalPos.anchoredPosition - new Vector2(500f, 0);
            bullet.DOAnchorPosX(bulletOriginalPos.anchoredPosition.x, reloadDuration).SetEase(Ease.OutQuad);
        }
        
    }

    public void MoveCylinder()
    {
        Image image = cylinder.GetComponent<Image>();
        Vector3 originalPosition = cylinder.anchoredPosition;
        Color originalColor = image.color;

        image.DOColor(Color.white, 0.1f);
        cylinder.DOAnchorPos(originalPosition + Vector3.right * 200f, 0.1f)
            .OnComplete(() =>
            {
                cylinder.DOAnchorPos(originalPosition, 0.1f);
                image.DOColor(originalColor, 0.1f);
            });
    }

    public void GrowAndShrinkCircles()
    {
        smallCircle.GrowAndShrink(0.1f);
        bigCircle.GrowAndShrink(0.1f);
    }

    void ShowCylinderAndCircles()
    {
        cylinder.DOAnchorPosX(0, 0.5f);
        circles.DOAnchorPosX(-52, 0.5f);
        LoadBullet();
    }
    void HideCylinderAndCircles()
    {
        cylinder.DOAnchorPosX(-200f, 0.5f);
        circles.DOAnchorPosX(-252f, 0.5f);
        UnLoadBullet();
    }
}
