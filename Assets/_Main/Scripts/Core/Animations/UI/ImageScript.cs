using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DIALOGUE;
using UnityEngine;
using UnityEngine.UI;

public class ImageScript : MonoBehaviour
{
    public static ImageScript instance { get; private set; }

    public Image overlayImage;
    public CanvasGroup blackFade;
    public Image whiteFlash;
    public CanvasGroup canvasGroup;
    public AudioClip flashSound;

    public Image background;
    public Dictionary<Character, GameObject> backgroundCharacters = new();

    public CanvasGroup animatedImageContainer;
    public VNAnimatedImage animatedImage;

    private void Awake()
    {
        blackFade.alpha = 1f;
        instance = this;
    }

    public void Show(Sprite image, bool flash, float duration = 0.4f)
    {
        GameStateManager.instance.uiState.overlayImage = new ImageState { spriteId = image.name, visible = true };
        overlayImage.sprite = image;
        OverlayTextBoxManager.instance.SetAsTextBox();
        OverlayTextBoxManager.instance.Show();
        ShowingOrHiding(canvasGroup, duration, 1f);

        if (flash)
        {
            Flash(duration, flashSound);
        }
    }

    public void Hide(bool flash, float duration = 0.4f)
    {
        GameStateManager.instance.uiState.overlayImage = new ImageState { spriteId = "", visible = false };
        OverlayTextBoxManager.instance.Hide();
        DialogueSystem.instance.UseInitialDialogueContainer();
        ShowingOrHiding(canvasGroup, duration, 0f);

        if (flash)
        {
            Flash(duration, flashSound);
        }
    }

    public void ShowBackground(Sprite sprite)
    {
        GameStateManager.instance.uiState.backgroundImage = new ImageState { spriteId = sprite.name, visible = true };
        Image oldBackground = background;
        Image newBackground = Instantiate(background, background.transform.parent);
        newBackground.sprite = sprite;

        Sequence seq = DOTween.Sequence();

        seq.Append(newBackground.DOFade(0f, 0f));
        seq.Append(newBackground.DOFade(1f, 0.2f).SetEase(Ease.Linear));
        seq.AppendCallback(() => Destroy(oldBackground.gameObject));
        seq.AppendCallback(() => background = newBackground);
    }

    public void HideBackground(float duration)
    {
        GameStateManager.instance.uiState.backgroundImage = new ImageState { spriteId = "", visible = false };
        background.DOFade(0f, duration).SetEase(Ease.Linear).OnComplete(() => background.sprite = null);
    }

    public void Flash(float duration, AudioClip sound)
    {
        SoundManager.instance.PlaySoundEffect(sound);
        whiteFlash.DOFade(1f, duration / 2).SetLoops(2, LoopType.Yoyo);
    }

    public void FadeToBlack(float duration)
    {
        ShowingOrHiding(blackFade, duration, 1f);
    }

    public void UnFadeToBlack(float duration)
    {
        ShowingOrHiding(blackFade, duration, 0f);
    }

    private void ShowingOrHiding(CanvasGroup canvasGroupToShowOrHide, float duration, float targetAlpha)
    {
        canvasGroupToShowOrHide.DOFade(targetAlpha, duration);
    }

    public void CreateAnimatedImage(VNAnimatedImage image)
    {
        animatedImage = Instantiate(image, animatedImageContainer.transform);
        animatedImageContainer.DOFade(1f, 0.2f).SetEase(Ease.Linear);
        GameStateManager.instance.uiState.animatedImage = new AnimatedImageState
            { prefabId = image.name, currentStateIndex = 0, visible = true };
    }

    public IEnumerator ForwardAnimatedImage()
    {
        if (animatedImage == null)
            yield break;

        GameStateManager.instance.uiState.animatedImage.currentStateIndex++;

        yield return animatedImage.ForwardSegment();
    }

    public void RemoveAnimatedImage(float duration)
    {
        if (animatedImage == null)
            return;

        GameStateManager.instance.uiState.animatedImage = new AnimatedImageState
            { prefabId = "", currentStateIndex = 0, visible = false };

        animatedImageContainer.DOFade(0f, duration).SetEase(Ease.Linear)
            .OnComplete(() => Destroy(animatedImage.gameObject));
    }

    public void ShowCharacterOnBackground(Character character)
    {
        if (backgroundCharacters.ContainsKey(character)) // If that character already exists, just show it
        {
            CanvasGroup canvas = backgroundCharacters[character].GetComponent<CanvasGroup>();
            canvas.alpha = 0f;
            canvas.DOFade(1f, 0.25f);
            GameStateManager.instance.uiState.characterStates
                .Find(characterState => characterState.character == character).visible = true;
            return;
        }

        GameStateManager.instance.uiState.characterStates.Add(new BackgroundCharacterState
            { character = character, visible = false });
        CreateCharacterOnBackground(character);

        GameObject characterObj = backgroundCharacters[character].gameObject;

        CanvasGroup canvasGroup = characterObj.GetComponent<CanvasGroup>();
        canvasGroup.DOFade(1f, 0.25f);
        GameStateManager.instance.uiState.characterStates
            .Find(characterState => characterState.character == character).visible = true;
    }

    public void CreateCharacterOnBackground(Character character)
    {
        GameObject characterObj = Instantiate(character.vnObjectPrefab, background.transform.parent);
        characterObj.transform.localPosition = Vector3.zero;

        characterObj.name = character.name;
        characterObj.transform.localPosition = new Vector3(
            0,
            character.vnObjectPrefab.transform.localPosition.y, 0);
        CanvasGroup canvas = characterObj.GetComponent<CanvasGroup>();
        canvas.alpha = 0f;
        
        backgroundCharacters.Add(character, characterObj);
    }

    public void HideCharacterOnBackground(Character character)
    {
        if (!backgroundCharacters.ContainsKey(character))
            return;

        CanvasGroup canvasGroup = backgroundCharacters[character].GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0f, 0.25f);
        GameStateManager.instance.uiState.characterStates
            .Find(characterState => characterState.character == character).visible = false;
    }

    public void DeleteCharacterOnBackground(Character character)
    {
        if (!backgroundCharacters.ContainsKey(character))
            return;

        Sequence seq = DOTween.Sequence();
        CanvasGroup canvasGroup = backgroundCharacters[character].GetComponent<CanvasGroup>();
        seq.Append(canvasGroup.DOFade(0f, 0.25f));
        seq.AppendCallback(() => Destroy(backgroundCharacters[character].gameObject));
        seq.AppendCallback(() => backgroundCharacters.Remove(character));
        BackgroundCharacterState state = GameStateManager.instance.uiState.characterStates
            .Find(characterState => characterState.character == character);
        GameStateManager.instance.uiState.characterStates.Remove(state);
    }
}