using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInAndOut : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float fadeDuration = 2f; // Time in seconds for a full fade in or out
    
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.PingPong(Time.time / fadeDuration, 1f);
        canvasGroup.alpha = t;
    }
}
