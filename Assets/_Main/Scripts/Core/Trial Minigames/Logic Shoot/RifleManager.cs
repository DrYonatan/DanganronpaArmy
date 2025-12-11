using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RifleManager : MonoBehaviour
{
    public RectTransform rifle;
    public RectTransform handle;
    public RectTransform stack;
    public RectTransform bulletChamber;
    public Image bulletInChamber;

    public int ammo;
    public bool isStackIn;
    public int bulletsInChamber;

    public bool isAlreadyPullingHandle;

    private Vector2 stackInPosition;
    private Vector2 stackOutPosition;

    void Start()
    {
        stackInPosition = stack.anchoredPosition;
        stackOutPosition = stackInPosition + new Vector2(14, -56);
    }

    public void RaiseRifle()
    {
        rifle.DOAnchorPosY(500, 0.2f);
    }

    public void PutRifleDown()
    {
        rifle.DOAnchorPosY(-500, 0.2f);
    }

    public void PullHandle()
    {
        if (isAlreadyPullingHandle)
            return;
        
        isAlreadyPullingHandle = true;
        handle.DOAnchorPosX(handle.anchoredPosition.x - 160, 0.2f).SetLoops(2, LoopType.Yoyo)
            .OnComplete(() => isAlreadyPullingHandle = false);

        if (!isStackIn)
        {
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
        stack.DOAnchorPos(stackInPosition, 0.2f);
        rifle.DOLocalRotate(new Vector3(0, 0, 15), 0.2f).SetLoops(2, LoopType.Yoyo);
        rifle.DOAnchorPosY(50, 0.2f).SetLoops(2, LoopType.Yoyo);
    }
    
    public void PullStackOut()
    {
        isStackIn = false;
        stack.DOAnchorPos(stackOutPosition, 0.2f);
    }

    private void UpdateBulletChamber()
    {
        switch (bulletsInChamber)
        {
            case 0:
                bulletChamber.anchoredPosition = new Vector2(-208, 75);
                bulletInChamber.DOFade(0f, 0f);
                break;
            case 1:
                bulletChamber.anchoredPosition = new Vector2(-96, 75);
                bulletInChamber.DOFade(1f, 0f);
                break;
            case 2:
                bulletChamber.anchoredPosition = new Vector2(-150, 75);
                bulletInChamber.DOFade(1f, 0f);
                break;
        }
    }
}