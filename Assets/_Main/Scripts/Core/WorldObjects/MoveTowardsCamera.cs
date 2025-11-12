using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveTowardsCamera : MonoBehaviour
{
    public float duration = 10f;
    public Transform cameraTransform;
    public TextMeshPro tmpText;
    public float glowFadeDuration = 5f;
    private Material tmpMaterial;
    private float elapsed = 0f;
    private Vector3 startPos;
    private Vector3 endPos;
    public AnimationCurve speedCurve = new AnimationCurve(
        new Keyframe(0f, 0f, 3f, 3f),
        new Keyframe(0.5f, 0.3f, 0f, 0f),
        new Keyframe(1f, 1f, 3f, 3f)
    );

    void Start()
    {
        StartCoroutine(FadeIn(0.5f));
        tmpMaterial = Instantiate(tmpText.fontMaterial);
        tmpText.fontMaterial = tmpMaterial;

        startPos = transform.position;
        endPos = new Vector3(
            cameraTransform.position.x,
            cameraTransform.position.y,
            cameraTransform.position.z
        );

        StartCoroutine(GlowEffect());
    }

    void Update()
    {
        if (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float curveT = speedCurve.Evaluate(t);
            transform.position = Vector3.Lerp(startPos, endPos, curveT);
        }
    }
    

    private IEnumerator GlowEffect()
    {
        Material mat = tmpText.fontMaterial;
        Color underlayColor = mat.GetColor("_UnderlayColor");
        float startAlpha = 0f;
        float targetAlpha = 1f;
        float elapsed = 0f;

        // Make sure starting alpha is 0
        underlayColor.a = startAlpha;
        mat.SetColor("_UnderlayColor", underlayColor);

        while (elapsed < glowFadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / glowFadeDuration);
            underlayColor.a = newAlpha;
            mat.SetColor("_UnderlayColor", underlayColor);
            yield return null;
        }

        // Ensure final value is set
        underlayColor.a = targetAlpha;
        mat.SetColor("_UnderlayColor", underlayColor);
        
        elapsed = 0f;
        while (elapsed < glowFadeDuration * 5f)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(targetAlpha, startAlpha, elapsed / (glowFadeDuration * 5f));
            underlayColor.a = newAlpha;
            mat.SetColor("_UnderlayColor", underlayColor);
            yield return null;
        }
    }

    IEnumerator FadeIn(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / duration);

            Color newColor = tmpText.color;
            newColor.a = alpha;
            tmpText.color = newColor;

            yield return null;
        }
    }
}