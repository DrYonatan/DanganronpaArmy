using System.Collections;
using UnityEngine;

public class FallEffect : RoomIntroEffect
{
    public float height = 5f;
    public float fallSpeed = 5f;
    public float delay = 0f;
    public AnimationCurve fallCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // for smooth falling

    private Vector3 targetPosition;

    public override IEnumerator PlayEffect()
    {
        targetPosition = transform.position;
        transform.position += new Vector3(0f, height, 0f);

        yield return new WaitForSeconds(delay);

        float duration = height / fallSpeed; // simple approximation
        float elapsed = 0f;
        Vector3 start = transform.position;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float yOffset = fallCurve.Evaluate(t); // 0 to 1
            transform.position = Vector3.Lerp(start, targetPosition, yOffset);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        // Optional: Little bounce after landing
        float bounceHeight = 0.2f;
        float bounceDuration = 0.2f;

        Vector3 bounceTop = targetPosition + new Vector3(0, bounceHeight, 0);
        elapsed = 0f;
        while (elapsed < bounceDuration)
        {
            float t = elapsed / bounceDuration;
            transform.position = Vector3.Lerp(targetPosition, bounceTop, Mathf.Sin(t * Mathf.PI)); // sine bounce
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
