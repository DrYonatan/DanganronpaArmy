using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveTowardsCamera : MonoBehaviour
{
    private float speed = 0.5f;
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
        float difference = transform.position.z - cameraTransform.position.z;
        if (difference < 0.35f)
        {
            speed = 2.5f;
        } 
        else if (difference > 0.35f && difference < 0.4f)
        {
            speed = 0.04f;
        }
        else
        {
            speed = 0.8f;
        }
        
        transform.position = Vector3.MoveTowards(
            transform.position,
            cameraTransform.position,
            speed * Time.deltaTime
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
        yield return new WaitForSeconds(0.7f);

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