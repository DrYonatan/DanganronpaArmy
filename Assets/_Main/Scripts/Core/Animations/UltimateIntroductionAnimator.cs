using System.Collections;
using CHARACTERS;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UltimateIntroductionAnimator : MonoBehaviour
{
    public Image background;
    public Image cylinder;
    public Image silhouetteMask;
    public Image silhouetteInsideImage;
    public Image whiteBar;
    public RectTransform blackBars;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI description;
    public float duration;
    public AudioClip soundEffect;

    private const string charactersLayerPath = "VN controller/Root/Canvas - Main/LAYERS/2 - Characters/Characters";

    private Color MakeColorTransparent(Color color)
    {
        color.a = 0f;
        return color;
    }

    private void Initialize(RectTransform characterTransform)
    {
        background.color = MakeColorTransparent(background.color);
        cylinder.rectTransform.anchoredPosition = new Vector2(-1500, 0);
        whiteBar.rectTransform.localScale = new Vector2(1, 0);
        blackBars.anchoredPosition = new Vector2(-2000, -407);
        silhouetteMask.rectTransform.anchoredPosition = new Vector2(-550f, characterTransform.anchoredPosition.y);
        silhouetteInsideImage.color =  MakeColorTransparent(silhouetteInsideImage.color);
        characterName.rectTransform.anchoredPosition += new Vector2(0, 30f);
        characterName.color = MakeColorTransparent(characterName.color);
        description.color = MakeColorTransparent(description.color);   
    }

    private void MainAnimation(RectTransform characterTransform, float initialCharacterX)
    {
        characterTransform.DOAnchorPosX(initialCharacterX - 500f, 0.3f);
        cylinder.rectTransform.DOAnchorPosX(690f, 0.3f).SetEase(Ease.OutBack).OnComplete(() => cylinder.rectTransform
            .DOLocalRotate(new Vector3(0f, 0f, -360f), 2f, RotateMode.FastBeyond360).SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental));
        blackBars.DOAnchorPosX(64f, 0.5f).SetEase(Ease.Linear);
        whiteBar.rectTransform.DOScaleY(1f, 0.2f).SetDelay(0.15f);
        silhouetteMask.rectTransform.DOAnchorPosY(silhouetteMask.rectTransform.anchoredPosition.y + 200f, duration).SetEase(Ease.Linear);
        characterName.rectTransform.DOAnchorPosY(characterName.rectTransform.anchoredPosition.y - 30f, 0.2f)
            .OnComplete(() => silhouetteInsideImage.DOFade(0.5f, 0.2f));
        characterName.DOFade(1f, 0.2f).SetDelay(0.1f);
        description.DOFade(1f, 0.2f).SetDelay(0.1f);
    }

    private void Finish(RectTransform characterTransform, float initialCharacterX)
    {
        cylinder.rectTransform.DOKill();
        cylinder.rectTransform.DOAnchorPosX(-1600f, 0.2f);
        whiteBar.rectTransform.DOScaleY(0f, 0.2f);
        whiteBar.DOFade(0f, 0.2f);
        blackBars.DOAnchorPosX(2200, 0.4f);
        silhouetteMask.DOFade(0f, 0.1f);
        characterTransform.DOAnchorPosX(initialCharacterX, 0.2f);
    }
    
    public IEnumerator Play(Character character, Color backgroundColor, string characterNameText,
        string descriptionText,
        Color characterNameColor, Color descriptionColor)
    {
        SetAttributes(character.FindStateByName("default").sprite, backgroundColor, characterNameText, descriptionText,
            characterNameColor,
            descriptionColor);

        RectTransform characterTransform =
            GameObject.Find($"{charactersLayerPath}/{character.name}").GetComponent<RectTransform>();
        CharacterState prevEmotion =
            character.FindStateBySprite(characterTransform.GetChild(0).GetComponent<Image>().sprite);
        VNCharacterManager.instance.SwitchEmotion(character, character.FindStateByName("default"));

        Initialize(characterTransform);
        
        yield return new WaitForSeconds(0.2f);

        SoundManager.instance.PlaySoundEffect(soundEffect);
        
        background.DOFade(1f, 0.1f);
        float initialCharacterX = characterTransform.anchoredPosition.x;
        
        MainAnimation(characterTransform, initialCharacterX);
        
        yield return new WaitForSeconds(duration);
        
        Finish(characterTransform, initialCharacterX);
        
        yield return new WaitForSeconds(0.1f);
        
        characterName.DOFade(0f, 0.05f);
        characterName.rectTransform.DOAnchorPosY(characterName.rectTransform.anchoredPosition.y + 30f, 0.05f);
        description.DOFade(0f, 0.05f);
        background.DOFade(0f, 0.2f);

        yield return new WaitForSeconds(0.5f);

        VNCharacterManager.instance.SwitchEmotion(character, prevEmotion);
    }


    private void SetAttributes(Sprite sprite, Color backgroundColor, string characterNameText, string descriptionText,
        Color characterNameColor, Color descriptionColor
    )
    {
        background.color = backgroundColor;
        cylinder.color = backgroundColor * 0.2f;
        silhouetteMask.sprite = sprite;
        silhouetteInsideImage.color = backgroundColor * 0.5f;
        characterName.color = characterNameColor;
        description.color = descriptionColor;
        characterName.text = characterNameText;
        description.text = descriptionText;
    }
}