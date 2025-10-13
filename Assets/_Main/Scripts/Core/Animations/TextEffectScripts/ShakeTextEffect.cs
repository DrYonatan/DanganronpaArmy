using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Text Effect/Shake")]
public class ShakeTextEffect : TextEffect
{
    [SerializeField] float intensity;
    public override IEnumerator Apply(Transform target)
    {
        target.DOShakePosition(
            duration: 1f,      
            strength: intensity,   
            vibrato: 20,      
            randomness: 90f, 
            snapping: false,     
            fadeOut: false       
        ).SetEase(Ease.Linear).SetLoops(-1);
        yield return null;
    }
}
