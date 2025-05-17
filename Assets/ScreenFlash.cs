using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    public Image flashImage;
    public float flashDuration = 5f;
    void Start()
    {
        if (flashImage != null)
        {
            flashImage.color = new Color(1, 1, 1, 0); // Ensure transparent at start
        }

        Flash();
    }
    
    void Flash()
    {
        StartCoroutine(DoFlash());
    }
    
    private IEnumerator DoFlash()
    {
        Color startColor = new Color(1, 1, 1, 0);
        Color endColor = new Color(1, 1, 1, 0.5f);
        float time = 0f;

        while (time < flashDuration)
        {
            flashImage.color = Color.Lerp(startColor, endColor, time / flashDuration);
            time += Time.deltaTime;
            yield return null;
        }

        flashImage.color = endColor; // Ensure it's exactly black at the end

        time = 0f;
        
        while (time < flashDuration)
        {
            flashImage.color = Color.Lerp(endColor, startColor, time / flashDuration);
            time += Time.deltaTime;
            yield return null;
        }
        
        flashImage.color = startColor; // Ensure it's exactly black at the end

    }
    
}
