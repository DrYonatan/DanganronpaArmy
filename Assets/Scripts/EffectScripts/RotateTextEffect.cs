using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Effect/Text Effect/Rotate")]
public class RotateTextEffect : TextEffect
{
    [SerializeField] float speed;
    public override void Apply(RectTransform target)
    {
        target.Rotate(Vector3.forward * Time.deltaTime * speed);
    }
}
