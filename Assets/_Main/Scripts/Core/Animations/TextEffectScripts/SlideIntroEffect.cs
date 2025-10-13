using System.Collections;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour Editor/Text Effect/Slide Intro")]

public class SlideIntroEffect : TextEffect
{
    public enum FromDirection
    {
        Right,
        Left,
        Up,
        Down,
        Camera
    }
    public FromDirection fromDirection = FromDirection.Right;
    public float duration;
    public override IEnumerator Apply(Transform target)
    {
        Vector3 initialPosition = target.localPosition;
        TeleportToFromDirection(fromDirection, target);
        target.DOLocalMove(initialPosition, duration);
        yield return new WaitForSeconds(duration);
    }

    void TeleportToFromDirection(FromDirection fromDirection, Transform target)
    {
        Vector3 direction = Vector3.zero;
        float moveAmount = 5f;

        switch (fromDirection)
        {
            case FromDirection.Right:
                direction = Vector3.right * moveAmount;
                break;
            case FromDirection.Left:
                direction = Vector3.left * moveAmount;
                break;
            case FromDirection.Up:
                direction = Vector3.up * moveAmount;
                break;
            case FromDirection.Down:
                direction = Vector3.down * moveAmount;
                break;
            case FromDirection.Camera:
                direction = Vector3.forward * -moveAmount;
                break;
        }
        
        target.localPosition += direction;
        
    }
}
