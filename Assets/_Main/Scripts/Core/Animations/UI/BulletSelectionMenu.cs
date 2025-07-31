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
    [SerializeField] private RectTransform cylinder;
    public bool isOpen;

    public void Appear()
    {
        rectTransform.anchoredPosition = new Vector2(0, 0);
        rectTransform.DOAnchorPosX(250f, startDuration);
        isOpen = true;
    }

    public void Disappear()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() => { UnLoadBullets(); });

        sequence.AppendCallback(() => { rectTransform.anchoredPosition = new Vector2(250f, 0); });

        sequence.Append(rectTransform.DOAnchorPosX(0, startDuration));
        isOpen = false;
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
            sequence.Append(cylinder.DOLocalRotate( new Vector3(0, 0, 60) * (i+1), 0.2f));
        }
    }

    public void OpenBullets()
    {
        int selectedIndex = GameLoop.instance.GetSelectedEvidenceIndex();
        int count = bullets.Count;
        int half = count / 2;
        float evenOffset = (count % 2 == 0) ? bulletSpacing / 2f : 0f;
        
        for (int i = 0; i < count; i++)
        {
            bullets[i].image.DOFade(1f, bulletsFadeDuration);
            bullets[i].text.DOFade(1f, bulletsFadeDuration);

            int offset = i - selectedIndex;

            // Wrap offset into the range [-half, half]
            if (offset > half)
                offset -= count;
            else if (offset < -half)
                offset += count;

            float yOffset = -offset * bulletSpacing + evenOffset;
            Vector2 targetPos = new Vector2(850, yOffset);
            RectTransform rt = bullets[i].GetComponent<RectTransform>();
            rt.DOAnchorPos(targetPos, 0.4f).SetEase(Ease.Linear);
            bullets[i].image.color = bullets[i].originalColor;
        }
        if(selectedIndex < bullets.Count && selectedIndex >= 0)
        SelectBullet(selectedIndex);
    }

    public void SelectBullet(int selectedIndex)
    {
        RectTransform rt = bullets[selectedIndex].GetComponent<RectTransform>();
        bullets[selectedIndex].image.color = bullets[selectedIndex].selectedColor;
        rt.DOAnchorPosX(rt.anchoredPosition.x + 100f, 0.4f);
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