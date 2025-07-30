using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BulletSelectionMenu : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] float startDuration;
    [SerializeField] private float bulletsFadeDuration;
    [SerializeField] float bulletSpacing;
    [SerializeField] List<UIBullet> bullets;
    [SerializeField] private UIBullet bulletPrefab;
    
    public void Appear()
    {
        rectTransform.anchoredPosition = new Vector2(0, 0);
        rectTransform.DOAnchorPosX(250f, startDuration);
    }

    public void Disappear()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() => {
            UnLoadBullets();
        });

        sequence.AppendCallback(() =>
        {
            rectTransform.anchoredPosition = new Vector2(250f, 0);
        });
        
        sequence.Append(rectTransform.DOAnchorPosX(0, startDuration));

    }

    public void LoadBullets(List<Evidence> evidences)
    {
        Sequence sequence = DOTween.Sequence();
        int count = evidences.Count;

        for (int i = 0; i < count; i++)
        {
            float yOffset = -((i - (count - 1) / 2f) * bulletSpacing);

            // Instantiate and place bullet
            UIBullet bulletGO = Instantiate(bulletPrefab, transform);
            bullets.Add(bulletGO);

            RectTransform rt = bulletGO.GetComponent<RectTransform>();

            // Final position
            Vector2 targetPos = new Vector2(850, yOffset);

            // Step 1: Teleport off-screen to the right (e.g., x = 2000)
            rt.anchoredPosition = new Vector2(2000, yOffset);

            // Set text
            bullets[i].text.text = evidences[i].Name;

            // Step 2: Animate to targetPos
            sequence.Append(rt.DOAnchorPos(targetPos, 0.4f).SetEase(Ease.Linear));
        }
    }

    public void UnLoadBullets()
    {
        foreach (UIBullet bullet in bullets)
        {
            bullet.image.DOFade(0f, bulletsFadeDuration);
            bullet.text.DOFade(0f, bulletsFadeDuration);
            bullet.GetComponent<RectTransform>().DOAnchorPos(new Vector2(850, 0), bulletsFadeDuration);
        }
    }
}
