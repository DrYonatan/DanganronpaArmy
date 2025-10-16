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
        GameObject parent = new GameObject();
        parent.name = "ShakeParent";
        parent.transform.position = target.position;
        parent.transform.rotation = target.rotation;

        List<Transform> children = new List<Transform>();
        foreach (Transform child in target)
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
            child.SetParent(parent.transform);
        }
        
        parent.transform.SetParent(target);
        
        parent.transform.DOShakePosition(
            duration: 1f,      
            strength: intensity,   
            vibrato: 30,      
            randomness: 90f, 
            snapping: false,     
            fadeOut: false       
        ).SetEase(Ease.Linear).SetLoops(-1).SetTarget(target);
        yield return null;
    }
}
