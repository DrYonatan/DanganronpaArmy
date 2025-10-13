using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviour Editor/Text Effect/Slide")]
public class Move : TextEffect
{
    [SerializeField] Vector3 moveVector = new Vector3(1f, 0, 0);
    public override IEnumerator Apply(Transform target)
    {
        target.DOMove(moveVector, 1f)
            .SetRelative().SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
        yield return null;
    }
}
