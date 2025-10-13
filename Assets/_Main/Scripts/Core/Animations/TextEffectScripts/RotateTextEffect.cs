using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Text Effect/Rotate")]
public class RotateTextEffect : TextEffect
{
    [SerializeField] float speed;
    public override IEnumerator Apply(Transform target)
    {
        target.DOLocalRotate(new Vector3(0f, 0f, -360f), 120 / speed, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental);
        yield return null;
    }
}
