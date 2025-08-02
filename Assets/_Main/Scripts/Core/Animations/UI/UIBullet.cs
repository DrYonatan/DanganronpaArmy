using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIBullet : MonoBehaviour
{
    public RectTransform imageTransform;

    public Color selectedColor;
    public Color originalColor = Color.white;

    public TextMeshProUGUI text;

    public Image image;

    public float extraWidth = 240f;

    public float shootDuration = 0.75f;
    
    const string WHITE_IMAGE_PATH = "Images/UI/whitebullet";
    const string REGULAR_IMAGE_PATH = "Images/UI/bullet";

    void Update()
    {
        float targetWidth = text.preferredWidth;

        imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            targetWidth / imageTransform.localScale.x + extraWidth);
    }

    public void Shoot()
    {
        image.sprite = Resources.Load<Sprite>(WHITE_IMAGE_PATH);
        image.color = Color.white;
        text.alpha = 0f;
        GetComponent<RectTransform>().DOAnchorPosX(imageTransform.anchoredPosition.x + 2000f, shootDuration).SetEase(Ease.Linear);
    }

    public void UnWhitenBullet()
    {
        text.alpha = 1f;
        image.color = selectedColor;
        image.sprite = Resources.Load<Sprite>(REGULAR_IMAGE_PATH);
    }
    
    
}