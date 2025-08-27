using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShatterNew : MonoBehaviour
{
    [SerializeField] private RawImage screenImage;
    [SerializeField] private CanvasGroup canvasGroup;
    public List<Image> pieces;
    public List<RawImage> pieceImages;
    public IEnumerator ScreenShatter()
    {
        yield return new WaitForEndOfFrame();
        Texture2D screenShot = ScreenCapture.CaptureScreenshotAsTexture();
        Texture2D newScreenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        newScreenShot.SetPixels(screenShot.GetPixels());
        newScreenShot.Apply();

        yield return new WaitForEndOfFrame();
        screenImage.texture = newScreenShot;
        screenImage.color = new Color(255, 255, 255, 1);
        canvasGroup.alpha = 1f;
        foreach (Image piece in pieces)
        {
            piece.color = new Color(255, 255, 255, 0);
        }
        yield return FlashPieces();
        
        screenImage.color = Color.black;
        foreach (RawImage piece in pieceImages)
        {
            piece.texture = newScreenShot;
        }
    }
    
    public IEnumerator FlashPieces()
    {
        pieces[1].color = new Color(255, 255, 255, 1);
        pieces[2].color = new Color(255, 255, 255, 1);
        yield return new WaitForSeconds(0.1f);
        pieces[1].color = new Color(255, 255, 255, 0);
        pieces[2].color = new Color(255, 255, 255, 0);
        yield return 4;
        
        
        pieces[0].color = new Color(255, 255, 255, 1);
        yield return new WaitForSeconds(0.1f);
        pieces[0].color = new Color(255, 255, 255, 0);
        yield return new WaitForEndOfFrame();
        
        yield return 4;
        
        pieces[0].color = new Color(255, 255, 255, 1);
        pieces[3].color = new Color(255, 255, 255, 1);
        yield return new WaitForSeconds(0.1f);
        pieces[0].color = new Color(255, 255, 255, 0);
        pieces[3].color = new Color(255, 255, 255, 0);
        yield return 4;

        foreach (Image piece in pieces)
        {
            piece.color = new Color(255, 255, 255, 1);
        }

    }
}
