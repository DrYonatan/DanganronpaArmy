using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class CharacterClickEffects : MonoBehaviour
{
    private bool isRunning = false;
    private Renderer[] renderers;
    private Color[] startColors;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if(!isRunning && !DialogueSystem.instance.isActive)
        {
          StartCoroutine(HopCharacter());
          MakeCharactersDisappear();
        }
    }

    void MakeCharactersDisappear()
    {
        GameObject parent = transform.parent.gameObject;
        // Get all renderers in this object and its children
        renderers = parent.GetComponentsInChildren<Renderer>();

        // Store original colors
        startColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            startColors[i] = renderers[i].material.color;
        }

        StartCoroutine(Fade(1f, 0f, 1f));
    }

    public void MakeCharactersReappear()
    {
        GameObject parent = transform.parent.gameObject;
        // Get all renderers in this object and its children
        renderers = parent.GetComponentsInChildren<Renderer>();

        // Store original colors
        startColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            startColors[i] = renderers[i].material.color;
        }

        StartCoroutine(Fade(0f, 1f, 0.5f));
    }

    IEnumerator HopCharacter()
    {
        isRunning = true;

        float elapsedTime = 0;
        float duration = 0.25f;

        Vector3 addPos = new Vector3(0, 4, 0);

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + addPos; // Adjust this vector to change the direction

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        elapsedTime = 0;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(targetPos, startPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos; // esnures it ends up in the initial position
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

    }
}
