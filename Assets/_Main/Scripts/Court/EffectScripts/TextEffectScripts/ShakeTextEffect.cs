using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Text Effect/Shake")]
public class ShakeTextEffect : TextEffect
{
    [SerializeField] float intensity;
    private Vector3 prevOffset;
    public override void Apply(RectTransform target)
    {
        float offsetX = Random.Range(-1f, 1f) * intensity;
        float offsetY = Random.Range(-1f, 1f) * intensity;

        Vector3 offset = new Vector3(offsetX, offsetY);
        target.localPosition += -prevOffset + offset;
        prevOffset = offset;
    }
}
