using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimationState
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public float opacity;
}
public class ComicSpriteAnimation : MonoBehaviour
{
    private RectTransform rectTransform;
    private Image image;
    public SpriteAnimationState startState;
    public SpriteAnimationState endState;
    public float duration;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Animate()
    {
        rectTransform.position = startState.position;
        rectTransform.DOAnchorPos(endState.position, duration);
        
        rectTransform.rotation = Quaternion.Euler(startState.rotation);
        rectTransform.DOLocalRotate(endState.rotation, duration);
        
        rectTransform.localScale = startState.scale;
        rectTransform.DOScale(endState.scale, duration);

        Color color = image.color;
        color.a = startState.opacity;
        image.color = color;
        image.DOFade(endState.opacity, duration);
    }
}
