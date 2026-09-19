using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SlideEffect : RoomIntroEffect
{
    public Vector3 offset = new Vector3(-3f, 0f, 0f); // Slide in from the left
    public float duration = 3f; // units per second

    private Vector3 targetPosition;

    public override IEnumerator PlayEffect()
    {
        targetPosition = transform.localPosition;
        transform.localPosition += offset;
        
        yield return new WaitForSeconds(delay);

        transform.DOLocalMove(targetPosition, duration);
    }
}
