using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Text Effect/Rotate")]
public class RotateTextEffect : TextEffect
{
    [SerializeField] float speed;
    public override void Apply(Transform target)
    {
        target.Rotate( Time.deltaTime * speed * Vector3.forward);
    }
}
