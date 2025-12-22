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
    
    public AudioClip shotSound;
    public AudioClip errorSound;
    
    public List<Button> buttons;

    public int ammo;
    public bool isStackIn;
    public int bulletsInChamber;
    public int stacksLeft = 5;

    public bool isAlreadyPullingHandle;
    public bool rifleErrorCooldown;

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

        else if (bulletsInChamber == 0 && isStackIn)
        {
            bulletsInChamber = 1;
        }

        UpdateBulletChamber();
    }

    public void PushStackIn()
    {
        isStackIn = true;
        LogicShootManager.instance.animator.switchStacksButton.interactable = false;
        stack.DOAnchorPos(stackInPosition, 0.2f).SetUpdate(true);
        rifle.localRotation = Quaternion.Euler(Vector3.zero);
        rifle.DOLocalRotate(new Vector3(0, 0, 15), 0.2f).SetUpdate(true).SetLoops(2, LoopType.Yoyo);
        rifle.DOAnchorPosY(50, 0.2f).SetUpdate(true).SetLoops(2, LoopType.Yoyo);
    }

    public void PullStackOut()
    {
        isStackIn = false;
        LogicShootManager.instance.animator.switchStacksButton.interactable = true;
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
        stack.DOAnchorPos(stackInPosition + new Vector2(2, -10), 0).SetUpdate(true);
    }

    public void RifleStuckTypeTwo()
    {
        bulletsInChamber = 2;
        UpdateBulletChamber();
    }

    public void OutOfAmmo()
    {
        bulletsInChamber = 0;
        UpdateBulletChamber();
        rifleErrorCooldown = true;
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

    public void TakeNewStack()
    {
        if (stacksLeft <= 0 || isStackIn)
            return;

        stacksLeft--;
        ammo = 30;

        Vector2 stackOriginalPosition = stack.anchoredPosition;

        Sequence seq = DOTween.Sequence();
        seq.Append(stack.DOAnchorPos(stackOriginalPosition + new Vector2(20, -1000), 0.5f).SetUpdate(true));
        seq.Join(stack.DOLocalRotate(new Vector3(0, 0, 360), 0.2f, RotateMode.FastBeyond360)
            .SetLoops(3, LoopType.Restart).SetUpdate(true));
        seq.AppendCallback(() => stack.anchoredPosition = stackOriginalPosition - new Vector2(0, 1000)).SetUpdate(true);
        seq.AppendCallback(() => LogicShootManager.instance.animator.RemoveStack(stacksLeft));
        seq.AppendCallback(() => LogicShootManager.instance.animator.UpdateAmmo(ammo));
        seq.Join(LogicShootManager.instance.animator.ammoNumberText.DOColor(Color.green, 0.1f)
            .SetLoops(2, LoopType.Yoyo).SetUpdate(true));
        seq.Append(stack.DOAnchorPosY(stackOriginalPosition.y, 0.3f)).SetUpdate(true);
    }
}