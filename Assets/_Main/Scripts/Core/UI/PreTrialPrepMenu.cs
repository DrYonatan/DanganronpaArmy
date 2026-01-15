using System.Collections.Generic;
using DG.Tweening;
using DIALOGUE;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreTrialPrepMenu : MonoBehaviour
{
    public Image cylinder;
    public RectTransform itemsContainer;
    public TextMeshProUGUI scrollingText;
    public TextMeshProUGUI chapterName;
    public List<TitleScreenMenuButton> items;

    public AudioClip menuMoveSound;
    public AudioClip selectionSound;
    public AudioClip music;

    public int currentItemIndex;

    public void Appear()
    {
        chapterName.text = GameStateManager.instance.chapters[GameStateManager.instance.chapterIndex].chapterName;
        MusicManager.instance.PlaySong(music);
        ImageScript.instance.UnFadeToBlack(0.1f);
        
        UpdateScrollingText();
        
        cylinder.rectTransform.DOLocalRotate(new Vector3(0, 0, 360), 8f, RotateMode.FastBeyond360).SetEase(Ease.Linear)
            .SetLoops(-1);
        itemsContainer.anchoredPosition += new Vector2(500, 0);
        itemsContainer.DOAnchorPosX(itemsContainer.anchoredPosition.x - 500, 0.4f);
    }

    private void UpdateScrollingText()
    {
        RectTransform textRect = scrollingText.rectTransform;
              
        scrollingText.text =
            GameStateManager.instance.chapters[GameStateManager.instance.chapterIndex].preTrialPrepText;
        
        scrollingText.ForceMeshUpdate();

        float textWidth = scrollingText.preferredWidth;
        
        textRect.DOAnchorPosX(textWidth, textWidth / 50)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    public void Disappear()
    {
        scrollingText.rectTransform.DOKill();
        cylinder.rectTransform.DOKill();
        cylinder.rectTransform.DOAnchorPosX(cylinder.rectTransform.anchoredPosition.x - 500f, 0.2f);
        itemsContainer.DOAnchorPosX(itemsContainer.anchoredPosition.x + 500f, 0.4f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            items[currentItemIndex].DisableHover();
            currentItemIndex = (currentItemIndex + 1) % items.Count;
            SoundManager.instance.PlaySoundEffect(menuMoveSound);
            items[currentItemIndex].HoverButtonAnimation();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            items[currentItemIndex].DisableHover();
            currentItemIndex = (currentItemIndex - 1 + items.Count) %
                               items.Count;
            SoundManager.instance.PlaySoundEffect(menuMoveSound);
            items[currentItemIndex].HoverButtonAnimation();
        }

        else if (PlayerInputManager.instance.DefaultInput())
        {
            SoundManager.instance.PlaySoundEffect(selectionSound);
            items[currentItemIndex].Click();
        }
    }
}