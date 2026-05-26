using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScaleEffect : RoomIntroEffect
{
    public Vector3 initialScale;
    public float duration;
    public override IEnumerator PlayEffect()
    {
        Vector3 normalScale = transform.localScale;
        transform.localScale = initialScale;
        yield return new WaitForSeconds(delay);
        transform.DOScale(normalScale, duration);
        yield return null;
    }
}
