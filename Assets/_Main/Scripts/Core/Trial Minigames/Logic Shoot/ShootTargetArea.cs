using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShootTargetArea : MonoBehaviour
{
    public List<RectTransform> holes = new List<RectTransform>();
    public bool isCorrect;
    protected Image image;
    public TextMeshProUGUI answer;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void CheckShoot()
    {
        if (isCorrect)
        {
            CorrectAnswer();
        }

        else
        {
            WrongAnswer();
        }
    }

    protected virtual void CorrectAnswer()
    {
        image.color = Color.green;
        if (holes.Count == 5)
        {
            ShootTarget parentTarget = transform.parent.GetComponent<ShootTarget>();
            parentTarget.SetIsDisappearing();
            SoundManager.instance.PlaySoundEffect(parentTarget.completeSound);
            transform.parent.DOKill();
            TextShatterExplosion explosion = Instantiate(LogicShootManager.instance.animator.successExplosion, LogicShootManager.instance.animator.targetsContainer);
            explosion.transform.GetChild(0).localScale = Vector3.one * 2f;
            explosion.transform.localPosition = transform.parent.localPosition;
            
            transform.parent.DOLocalRotate(new Vector3(0, 360, 0), 0.1f, RotateMode.FastBeyond360).SetLoops(4)
                .OnComplete(() => parentTarget.DisappearAnimation()).SetLink(parentTarget.gameObject);
            CalculateGrouping();
        }
    }

    protected virtual void WrongAnswer()
    {
        if(LogicShootManager.instance.isActive)
           LogicShootManager.instance.DamagePlayer(0.5f);
        image.DOColor(Color.red, 0.05f).SetLoops(5, LoopType.Yoyo).SetLink(transform.parent.gameObject).OnComplete(() => image.color = Color.red);
    }

    private void CalculateGrouping()
    {
        float sum = 0;
        int count = 0;

        for (int i = 0; i < holes.Count; i++)
        {
            for (int j = i + 1; j < holes.Count; j++)
            {
                sum += Vector3.Distance(holes[i].localPosition, holes[j].localPosition);
                count++;
            }
        }

        float avgDistance = sum / count;
        int mikbaz = (int)Mathf.Floor(avgDistance / 10);
        LogicShootManager.instance.animator.ShowMikbazText(mikbaz);
    }
}