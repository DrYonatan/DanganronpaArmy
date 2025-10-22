using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Text Effect/Slide Outro")]
public class SlideOutroEffect : TextEffect
{
    public enum ToDirection
    {
        Right,
        Left,
        Up,
        Down,
        Camera
    }
    public ToDirection toDirection = ToDirection.Right;
    public float duration;
    
    public override IEnumerator Apply(Transform target)
    {
        Vector3 initialPosition = target.localPosition;
        target.DOLocalMove(GetTargetPosition(toDirection, initialPosition), duration);
        yield return new WaitForSeconds(duration);
    }
    
    Vector3 GetTargetPosition(ToDirection toDirection, Vector3 startPosition)
    {
        Vector3 direction = Vector3.zero;
        float moveAmount = 5f;

        switch (toDirection)
        {
            case ToDirection.Right:
                direction = Vector3.right * moveAmount;
                break;
            case ToDirection.Left:
                direction = Vector3.left * moveAmount;
                break;
            case ToDirection.Up:
                direction = Vector3.up * moveAmount;
                break;
            case ToDirection.Down:
                direction = Vector3.down * moveAmount;
                break;
            case ToDirection.Camera:
                direction = Vector3.forward * -moveAmount;
                break;
        }
        
        return startPosition + direction;
        
    }
}
