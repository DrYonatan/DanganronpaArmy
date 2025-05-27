using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveTowardsCamera : MonoBehaviour
{
    public float speed = 0.5f;
    public Transform cameraTransform;
    public TextMeshPro tmpText;
    public Color glowColor = Color.cyan;
    public float maxOutlineWidth = 0.9f;
    public float glowFadeDuration = 5f;
    private Material tmpMaterial;
    private float originalOutlineWidth;
    private Color originalOutlineColor;

    void Start()
    {
        StartCoroutine(FadeIn(0.5f));
        tmpMaterial = Instantiate(tmpText.fontMaterial);
        tmpText.fontMaterial = tmpMaterial;

        originalOutlineWidth = tmpMaterial.GetFloat(ShaderUtilities.ID_OutlineWidth);
        originalOutlineColor = tmpMaterial.GetColor(ShaderUtilities.ID_OutlineColor);

        StartCoroutine(GlowEffect());
    }

    void Update()
    {
        float speedFactor = transform.position.z - cameraTransform.position.z < 0.4f ? 2 : 0.4f;
        transform.position = Vector3.MoveTowards(
            transform.position,
            cameraTransform.position,
            speedFactor * speed * Time.deltaTime
        );
    }

    private IEnumerator GlowEffect()
    {
        float timer = 0f;

        // Fade In
        while (timer < glowFadeDuration)
        {
            float t = timer / glowFadeDuration;
            tmpMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, Mathf.Lerp(originalOutlineWidth, maxOutlineWidth, t));

            Color fadeColor = Color.Lerp(originalOutlineColor, glowColor, t);
            tmpMaterial.SetColor(ShaderUtilities.ID_OutlineColor, fadeColor);

            timer += Time.deltaTime;
            yield return null;
        }

        tmpMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, maxOutlineWidth);
        tmpMaterial.SetColor(ShaderUtilities.ID_OutlineColor, glowColor);

        // Hold glow for a brief moment
        yield return new WaitForSeconds(0.5f);

        // Fade Out
        timer = 0f;
        while (timer < glowFadeDuration)
        {
            float t = timer / glowFadeDuration;
            tmpMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, Mathf.Lerp(maxOutlineWidth, originalOutlineWidth, t));

            Color fadeColor = Color.Lerp(glowColor, originalOutlineColor, t);
            tmpMaterial.SetColor(ShaderUtilities.ID_OutlineColor, fadeColor);

            timer += Time.deltaTime;
            yield return null;
        }

        tmpMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, originalOutlineWidth);
        tmpMaterial.SetColor(ShaderUtilities.ID_OutlineColor, originalOutlineColor);
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