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
        StartCoroutine(WrongPipeline());
    }

    private IEnumerator CorrectPipeline()
    {
        LogicShootManager.instance.FinishGame();

        yield return new WaitForSeconds(0.8f);

        TextShatterExplosion explosion = Instantiate(LogicShootManager.instance.animator.successExplosion,
            LogicShootManager.instance.animator.targetsContainer);
        explosion.transform.GetChild(0).localScale = Vector3.one * 2f;
        explosion.transform.localPosition = transform.parent.localPosition;

        transform.parent.DOLocalRotate(new Vector3(0, 360, 0), 0.1f, RotateMode.FastBeyond360).SetLoops(4)
            .OnComplete(() => transform.parent.GetComponent<ShootTarget>().DisappearAnimation());
    }

    private IEnumerator WrongPipeline()
    {
        yield return new WaitForSeconds(0.5f);
        transform.parent.DOKill();
        Destroy(transform.parent.gameObject);
        LogicShootManager.instance.ReturnToGame();
    }
}