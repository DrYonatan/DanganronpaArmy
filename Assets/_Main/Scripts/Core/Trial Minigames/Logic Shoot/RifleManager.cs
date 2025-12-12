using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RifleManager : MonoBehaviour
{
    public RectTransform rifle;
    public RectTransform handle;
    public RectTransform stack;
    public RectTransform bulletChamber;
    public Image bulletInChamber;
    public RectTransform flyingBullet;

    public List<Button> buttons;

    public int ammo;
    public bool isStackIn;
    public int bulletsInChamber;

    public bool isAlreadyPullingHandle;

    private Vector2 stackInPosition;
    private Vector2 stackOutPosition;
    private Vector2 flyingBulletOriginalPosition;

    public void InitializeRifle()
    {
        stackInPosition = stack.anchoredPosition;
        stackOutPosition = stackInPosition + new Vector2(14, -56);
        flyingBulletOriginalPosition = flyingBullet.anchoredPosition;
        flyingBullet.anchoredPosition -= new Vector2(0, 1000);
        ammo = 30;
    }

    public void RaiseRifle()
    {
        rifle.DOAnchorPosY(0, 0.2f).SetUpdate(true).OnComplete(() => FadeAllButtons(1f));
    }

    public void PutRifleDown()
    {
        FadeAllButtons(0f);
        rifle.DOAnchorPosY(-1000, 0.2f).SetUpdate(true);
    }

    private void FadeAllButtons(float to)
    {
        foreach (Button button in buttons)
        {
            button.image.DOFade(to, 0.1f).SetUpdate(true);
        }
    }

    public void PullHandle()
    {
        if (isAlreadyPullingHandle)
            return;

        isAlreadyPullingHandle = true;
        handle.DOAnchorPosX(handle.anchoredPosition.x - 160, 0.2f).SetUpdate(true).SetLoops(2, LoopType.Yoyo)
            .OnComplete(() => isAlreadyPullingHandle = false);

        if (!isStackIn && bulletsInChamber > 0)
        {
            DropBullet();
            bulletsInChamber = 0;
        }

        else if (bulletsInChamber == 0)
        {
            bulletsInChamber = 1;
        }

        UpdateBulletChamber();
    }

    public void PushStackIn()
    {
        isStackIn = true;
        stack.DOAnchorPos(stackInPosition, 0.2f).SetUpdate(true);
        rifle.DOLocalRotate(new Vector3(0, 0, 15), 0.2f).SetUpdate(true).SetLoops(2, LoopType.Yoyo);
        rifle.DOAnchorPosY(50, 0.2f).SetUpdate(true).SetLoops(2, LoopType.Yoyo);
    }

    public void PullStackOut()
    {
        isStackIn = false;
        stack.DOAnchorPos(stackOutPosition, 0.2f).SetUpdate(true);
    }

    private void UpdateBulletChamber()
    {
        switch (bulletsInChamber)
        {
            case 0:
                bulletChamber.anchoredPosition = new Vector2(-208, 75);
                bulletInChamber.DOFade(0f, 0f).SetUpdate(true);
                break;
            case 1:
                bulletChamber.anchoredPosition = new Vector2(-96, 75);
                bulletInChamber.DOFade(1f, 0f).SetUpdate(true);
                break;
            case 2:
                bulletChamber.anchoredPosition = new Vector2(-150, 75);
                bulletInChamber.DOFade(1f, 0f).SetUpdate(true);
                break;
        }
    }

    public void RifleStuckTypeOne()
    {
        isStackIn = false;
        stack.DOAnchorPos(stackInPosition + new Vector2(2, 30), 0).SetUpdate(true);
    }

    public void RifleStuckTypeTwo()
    {
        bulletsInChamber = 2;
        UpdateBulletChamber();
    }

    public bool IsRifleIntact()
    {
        return ammo > 0 && bulletsInChamber == 1 && isStackIn;
    }

    private void DropBullet()
    {
        flyingBullet.anchoredPosition = flyingBulletOriginalPosition;
        flyingBullet.DOAnchorPos(flyingBulletOriginalPosition + new Vector2(-30, -1000), 1f).SetUpdate(true);
        flyingBullet.DOLocalRotate(new Vector3(0, 0, 360), 0.2f, RotateMode.FastBeyond360).SetLoops(3, LoopType.Restart)
            .SetUpdate(true);
    }
}