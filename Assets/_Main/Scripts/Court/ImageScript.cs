using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageScript : MonoBehaviour
{
    public static ImageScript instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }
    public void Show()
    {
        StartCoroutine(ShowImage());
    }
    public void Hide()
    {
        StartCoroutine(HideImage());
    }



    public static IEnumerator ShowImage()
    {
        CanvasGroup self = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/3 - Cinematic/Image").GetComponent<CanvasGroup>();
        float targetAlpha = 1;
        
        while (self.alpha != targetAlpha)
        {
            self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, 5f * Time.deltaTime);
            yield return null;
        }

    }

    public static IEnumerator HideImage()
    {
        CanvasGroup self = GameObject.Find("VN controller/Root/Canvas - Main/LAYERS/3 - Cinematic/Image").GetComponent<CanvasGroup>();
        float targetAlpha = 0;

        while (self.alpha != targetAlpha)
        {
            self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, 5f * Time.deltaTime);
            yield return null;
        }

    }

  
   
}
