using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;



public class ScreenShatterNew : MonoBehaviour
{ 
    [Serializable]
    class FlashGroup 
    {
        public List<Image> pieces = new ();
    }
    
    [SerializeField] private RawImage screenImage;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private List<FlashGroup> flashGroups = new ();
    [SerializeField] private List<ScreenPiece> pieces;
    [SerializeField] private RawImage blackImage;
    [SerializeField] private PostProcessVolume  psVolume;
    [SerializeField] private GameObject breakText;

    public IEnumerator ScreenShatter()
    {
        yield return new WaitForEndOfFrame();
        Texture2D screenShot = ScreenCapture.CaptureScreenshotAsTexture();
        Texture2D newScreenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        newScreenShot.SetPixels(screenShot.GetPixels());
        newScreenShot.Apply();
        
        ChangePiecesTransparency(0f);
        yield return new WaitForEndOfFrame();
        screenImage.texture = newScreenShot;
        screenImage.color = new Color(255, 255, 255, 1);
        canvasGroup.alpha = 1f;
        GameLoop.instance.debateUIAnimator.gameObject.SetActive(false);
        yield return FlashPieces();
        ChangePiecesTransparency(1f);
        
        screenImage.color = Color.black;
        foreach (ScreenPiece piece in pieces)
        {
            piece.image.texture = newScreenShot;
        }

        yield return new WaitForSeconds(0.25f);
        yield return Shatter();
    }
    
    public IEnumerator FlashPieces()
    {
        foreach(FlashGroup flashGroup in flashGroups)
        {
            if (flashGroup.pieces.Count == 0)
            {
                yield return new WaitForSeconds(0.05f);
                continue;
            }
            foreach (Image piece in flashGroup.pieces)
            {
                piece.color = new Color(255, 255, 255, 1);
            }
            yield return new WaitForSeconds(0.05f);
            foreach (Image piece in flashGroup.pieces)
            {
                piece.color = new Color(255, 255, 255, 0);
            }
        }
        
    }

    public void ChangePiecesTransparency(float transparency)
    {
        foreach (FlashGroup flashGroup in flashGroups)
        {
            foreach (Image piece in flashGroup.pieces)
            {
                piece.color = new Color(255, 255, 255, transparency);
            }
        }
    }

    IEnumerator Shatter()
    {
        foreach (ScreenPiece piece in pieces)
        {
            StartCoroutine(piece.Move(3f));
        }

        screenImage.texture = null;
        screenImage.DOColor(Color.white, 0.8f)
            .SetLoops(2, LoopType.Yoyo);
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            psVolume.weight =  Mathf.Lerp(0f, 0.8f, elapsedTime / 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        breakText.SetActive(true);

        elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            psVolume.weight =  Mathf.Lerp(0.8f, 0f, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        blackImage.DOColor(Color.black, 1f);
        yield return new WaitForSeconds(2f);
        ImageScript.instance.FadeToBlack(0f);
        canvasGroup.alpha = 0;
    }
    
}
