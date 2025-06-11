using System.Collections;
using UnityEngine;

public class SlideEffect : MonoBehaviour
{
    public Vector3 offset = new Vector3(-3f, 0f, 0f); // Slide in from the left
    public float speed = 3f; // units per second
    public float delay = 0f;

    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
        transform.position += offset;
        StartCoroutine(PlayEffect());
    }

    private IEnumerator PlayEffect()
    {
        yield return new WaitForSeconds(delay);

        Vector3 start = transform.position;
        float distance = Vector3.Distance(start, targetPosition);
        float duration = distance / speed;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(start, targetPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Ensure final position is exact
    }
}
