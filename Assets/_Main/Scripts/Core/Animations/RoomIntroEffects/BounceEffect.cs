using System.Collections;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
    public float delay = 0f;
    public float duration = 1f;
    public float bounceFrequency = 2f; // number of bounces
    public float damping = 5f;         // how quickly it settles
    private Renderer renderer;
    void Start()
    {
        renderer = GetComponent<Renderer>();
        StartCoroutine(PlayEffect());
    }

    private IEnumerator PlayEffect()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 startScale = new Vector3(originalScale.x, 0.1f, originalScale.z);
        transform.localScale = startScale;

        // Store the original bottom position
        float baseY = transform.position.y - (originalScale.y / 2f);
        renderer.enabled = false;
        yield return new WaitForSeconds(delay);
        renderer.enabled = true;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Damped sine wave
            float bounce = Mathf.Exp(-damping * t) * Mathf.Sin(bounceFrequency * t * Mathf.PI * 2f);
            float yScale = Mathf.Lerp(0.1f, originalScale.y, 1f - Mathf.Abs(bounce));

            transform.localScale = new Vector3(originalScale.x, yScale, originalScale.z);

            // Adjust position to keep the bottom anchored
            float newY = baseY + (yScale / 2f);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            yield return null;
        }

        // Finalize scale and position
        transform.localScale = originalScale;
        transform.position = new Vector3(transform.position.x, baseY + originalScale.y / 2f, transform.position.z);
    }


   
}