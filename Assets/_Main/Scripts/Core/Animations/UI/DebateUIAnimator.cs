using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DIALOGUE;
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
    public List<Image> indicators = new List<Image>();
    public DialogueContainer dialogueContainer = new DialogueContainer();

    public RawImage fadeScreenshotImage;

    public RectTransform cylinder;
    public RectTransform bullet;
    public RectTransform bulletOriginalPos;

    public RectTransform circles;
    public DebateCircle smallCircle;
    public DebateCircle bigCircle;

    public BulletSelectionMenu bulletSelectionMenu;

    public float moveAmountY = 40f;
    public float moveAmountX = 150f;

    public float duration = 0.2f;
    public float reloadDuration = 0.2f;

    public void DebateUIAppear()
    {
        namePart.anchoredPosition = namePartOriginalPos.anchoredPosition + new Vector2(0, -moveAmountY);
        facePart.anchoredPosition = facePartOriginalPos.anchoredPosition + new Vector2(moveAmountX, 0);
        timePart.anchoredPosition = timePartOriginalPos.anchoredPosition + new Vector2(0, moveAmountY);

        Sequence seq = DOTween.Sequence();

        seq.Append(namePart.DOAnchorPos(namePartOriginalPos.anchoredPosition, duration).SetEase(Ease.OutQuad));
        seq.Append(facePart.DOAnchorPos(facePartOriginalPos.anchoredPosition, duration).SetEase(Ease.OutQuad));
        seq.Append(timePart.DOAnchorPos(timePartOriginalPos.anchoredPosition, duration).SetEase(Ease.OutQuad));

        seq.OnComplete(() => TrialManager.instance.barsAnimator.ShowDebateBars(0.2f));

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
            HideCylinderAndCircles(0.5f);
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

    public void FadeFromAngleToAngle()
    {
        Texture2D screenShot = ScreenCapture.CaptureScreenshotAsTexture();
        Texture2D newScreenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        newScreenShot.SetPixels(screenShot.GetPixels());
        newScreenShot.Apply();
        fadeScreenshotImage.texture = newScreenShot;
        fadeScreenshotImage.color = Color.white;
        fadeScreenshotImage.DOFade(0f, 0.8f);
    }

    public void DebateUIDisappear()
    {
        namePart.anchoredPosition = namePartOriginalPos.anchoredPosition + new Vector2(0, -moveAmountY);
        facePart.anchoredPosition = facePartOriginalPos.anchoredPosition + new Vector2(moveAmountX, 0);
        timePart.anchoredPosition = timePartOriginalPos.anchoredPosition + new Vector2(0, moveAmountY);
        bullet.DOAnchorPosX(bulletOriginalPos.anchoredPosition.x - 300f, 0).SetEase(Ease.OutQuad);
        HideCylinderAndCircles(0f);
        TrialCursorManager.instance.Hide();
    }

    public void ChangeFace(Sprite sprite)
    {
        characterFace.SetFace(sprite);
    }

    void GenerateIndicators()
    {
        // Clear old indicators
        foreach (Transform child in nodeIndicatorContainer)
        {
            child.GetComponent<Image>().DOKill();
            Destroy(child.gameObject);
        }

        indicators.Clear();

        for (int i = 0; i < GameLoop.instance.debateSegment.dialogueNodes.Count; i++)
        {
            GameObject indicator = Instantiate(nodeIndicatorPrefab, nodeIndicatorContainer);
            Image image = indicator.GetComponent<Image>();
            image.color = Color.gray;
            indicators.Add(image);
        }
    }

    public void HighlightNode(int index)
    {
        UnHighlightAllNodes();
        if (index < indicators.Count)
        {
            indicators[index].DOColor(new Color(1f, 1f, 0.5f), 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine).SetUpdate(true);
        }
        
    }

    public void UnHighlightAllNodes()
    {
        foreach (Image indicator in indicators)
        {
            indicator.color = Color.gray;
            indicator.DOComplete(); // Stop any running tweens
            indicator.DOKill();
        }
    }

    public void UpdateName(string characterName)
    {
        dialogueContainer.nameContainer.Show(characterName);
    }

    void UnLoadBullet()
    {
        bullet.DOAnchorPosX(bulletOriginalPos.anchoredPosition.x - 300f, reloadDuration).SetEase(Ease.OutQuad);
    }

    public void LoadBullet()
    {
        bullet.GetComponent<UIBullet>().UnWhitenBullet();
        bullet.anchoredPosition = bulletOriginalPos.anchoredPosition - new Vector2(500f, 0);
        bullet.DOAnchorPosX(bulletOriginalPos.anchoredPosition.x, reloadDuration).SetEase(Ease.OutQuad).SetUpdate(true);
    }

    public void MoveCylinder()
    {
        Image image = cylinder.GetComponent<Image>();
        Vector3 originalPosition = cylinder.anchoredPosition;
        Color originalColor = image.color;

        image.DOColor(Color.white, 0.1f).SetUpdate(true);
        cylinder.DOAnchorPos(originalPosition + Vector3.right * 200f, 0.1f).SetUpdate(true)
            .OnComplete(() =>
            {
                cylinder.DOAnchorPos(originalPosition, 0.1f).SetUpdate(true);
                image.DOColor(originalColor, 0.1f).SetUpdate(true);
            });
    }

    public void GrowAndShrinkCircles()
    {
        smallCircle.GrowAndShrink(0.1f);
        bigCircle.GrowAndShrink(0.1f);
    }

    public void ShowCylinderAndCircles()
    {
        cylinder.DOAnchorPosX(0, 0.5f).SetUpdate(true);
        circles.DOAnchorPosX(-52, 0.5f).SetUpdate(true);
        LoadBullet();
    }

    public void HideCylinderAndCircles(float hideDuration)
    {
        cylinder.DOAnchorPosX(-200f, hideDuration).SetUpdate(true);
        circles.DOAnchorPosX(-252f, hideDuration).SetUpdate(true);
        UnLoadBullet();
    }

    public void ShowTextBox()
    {
        namePart.DOAnchorPosY(0, 0.3f);
        facePart.DOAnchorPos(new Vector2(26.57f, 125), 0.3f);
    }

    public void HideTextBox()
    {
        namePart.DOAnchorPosY(namePartOriginalPos.anchoredPosition.y, 0.3f);
        facePart.DOAnchorPos(facePartOriginalPos.anchoredPosition, 0.3f);
    }
}