using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviour Editor/Text Effect/Slide")]
public class Move : TextEffect
{
    [SerializeField] Vector3 moveVector;
    public override void Apply(Transform target)
    {
        target.position += moveVector * Time.deltaTime/3;
    }
}
