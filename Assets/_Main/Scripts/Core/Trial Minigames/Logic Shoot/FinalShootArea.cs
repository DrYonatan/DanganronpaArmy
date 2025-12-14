using System.Collections;
using DG.Tweening;
using UnityEngine;

public class FinalShootArea : ShootTargetArea
{
    protected override void CorrectAnswer()
    {
        image.color = Color.green;
        StartCoroutine(CorrectPipeline());
    }

    protected override void WrongAnswer()
    {
        base.WrongAnswer();
        LogicShootManager.instance.ReturnToGame();
    }

    private IEnumerator CorrectPipeline()
    {
        yield return LogicShootManager.instance.FinishGame();
        transform.parent.DOLocalRotate(new Vector3(0, 360, 0), 0.1f, RotateMode.FastBeyond360).SetLoops(4)
            .OnComplete(() => transform.parent.GetComponent<ShootTarget>().DisappearAnimation());
    }
}