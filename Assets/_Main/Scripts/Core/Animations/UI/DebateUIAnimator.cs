using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

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

    public float moveAmountY = 40f;
    public float moveAmountX = 150f;

    public float duration = 0.5f;

    public void DebateUIAppear()
    {
        namePart.anchoredPosition = namePartOriginalPos.anchoredPosition + new Vector2(0, -moveAmountY);
        facePart.anchoredPosition = facePartOriginalPos.anchoredPosition + new Vector2(moveAmountX, 0);
        timePart.anchoredPosition = timePartOriginalPos.anchoredPosition + new Vector2(0, moveAmountY);

        namePart.DOAnchorPos(namePartOriginalPos.anchoredPosition, duration).SetEase(Ease.OutQuad);
        facePart.DOAnchorPos(facePartOriginalPos.anchoredPosition, duration).SetEase(Ease.OutQuad);
        timePart.DOAnchorPos(timePartOriginalPos.anchoredPosition, duration).SetEase(Ease.OutQuad);
        GenerateIndicators();
    }
    public void DebateUIDisappear()
    {
        namePart.anchoredPosition = namePartOriginalPos.anchoredPosition + new Vector2(0, -moveAmountY);
        facePart.anchoredPosition = facePartOriginalPos.anchoredPosition + new Vector2(moveAmountX, 0);
        timePart.anchoredPosition = timePartOriginalPos.anchoredPosition + new Vector2(0, moveAmountY);
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
}
