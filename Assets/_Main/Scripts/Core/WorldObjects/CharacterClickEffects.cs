using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using DIALOGUE;

public class CharacterClickEffects : MonoBehaviour
{
    private bool isRunning = false;
    private Renderer[] renderers;
    private Color[] startColors;
    public static CharacterClickEffects instance { get; private set; }

    private Coroutine fadeRoutine;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            DestroyImmediate(gameObject);
    }

    public IEnumerator Interact(Transform characterTransform)
    {
        if (!isRunning && !DialogueSystem.instance.isActive)
        {
            yield return HopCharacter(characterTransform);
            MakeCharactersDisappear(characterTransform.parent.GetComponent<WorldCharactersParent>(), 1f);
        }
    }

    public void MakeCharactersDisappear(WorldCharactersParent characters, float duration)
    {
        if (characters != null)
        {
            renderers = characters.GetComponentsInChildren<Renderer>();

            // Store original colors
            startColors = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                startColors[i] = renderers[i].material.color;
            }

            StartCoroutine(Fade(1f, 0f, duration));
        }
    }

    public void MakeCharactersReappear(GameObject characters)
    {
        if (characters != null)
        {
            // Get all renderers in this object and its children
            renderers = characters.GetComponentsInChildren<Renderer>();

            // Store original colors
            startColors = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                startColors[i] = renderers[i].material.color;
            }

            if(fadeRoutine != null)
                StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(Fade(renderers[0].material.color.a, 1f, 0.5f));
        }
    }

    IEnumerator HopCharacter(Transform characterTransform)
    {
        isRunning = true;

        float duration = 0.3f;

        Vector3 startPos = characterTransform.position;

        Sequence seq = DOTween.Sequence();

        seq.Append(characterTransform.DOMoveY(startPos.y + 1.2f, 0.18f)
            .SetEase(Ease.OutQuad));

        seq.Append(characterTransform.DOMoveY(startPos.y, 0.12f)
            .SetEase(Ease.InQuad));

        seq.Append(characterTransform.DOMoveY(startPos.y + 0.7f, 0.12f)
            .SetEase(Ease.OutQuad));

        seq.Append(characterTransform.DOMoveY(startPos.y, 0.10f)
            .SetEase(Ease.InQuad));

        seq.Append(characterTransform.DOMoveY(startPos.y + 0.2f, 0.08f)
            .SetEase(Ease.OutQuad));

        seq.Append(characterTransform.DOMoveY(startPos.y, 0.08f)
            .SetEase(Ease.InQuad));

        yield return new WaitForSeconds(duration);

        characterTransform.position = startPos;
        isRunning = false;
    }

    IEnumerator Fade(float start, float target, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(start, target, elapsedTime / duration);

            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null)
                {
                    Color newColor = startColors[i];
                    newColor.a = alpha;
                    renderers[i].material.color = newColor;
                }
            }

            yield return null;
        }

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
            {
                Color newColor = startColors[i];
                newColor.a = target;
                renderers[i].material.color = newColor;
            }
        }
    }
}