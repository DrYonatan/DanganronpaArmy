using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager instance { get; private set; }

    public const string charactersLayerPath = "VN controller/Root/Canvas - Main/LAYERS/2 - Characters";

    private void Awake()
    {
        instance = this;
    }

    public void MoveCamera(string direction, float duration)
    {
        StartCoroutine(MoveCamera(duration, direction));

    }

    public void ZoomCamera(string zoom)
    {
        StartCoroutine(Zoom(zoom));
    }

    IEnumerator Zoom(string zoom)
    {
        float z = 0;
        float elapsedTime = 0;
        if (zoom.ToLower() == "in")
            z = 15;
        else
            z = 26;
        while (Camera.main.fieldOfView != z)
        {
            Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, z, 60 * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.fieldOfView = z;




    }  

    IEnumerator MoveCamera(float duration, string location)
    {
        GameObject characters = GameObject.Find(charactersLayerPath);
        float elapsedTime = 0;
        int x = 0;
        int characterX = 0;
        switch (location) 
        {
           case "right" :
           x = 20;
           characterX = -403;
           break;

           case "middle" :
           x = 3;
           characterX = -25;
           break;

           case "left" :
           x = -20;
           characterX = 350;
           break;
        }

        Vector3 startPos = Camera.main.transform.position;
        Vector3 targetPos = new Vector3(x, 19, -245);

        Vector3 charactersStartPos = characters.transform.localPosition;
        Vector3 charactersTargetPos = new Vector3(characterX, 274, 0);


        while (elapsedTime < duration)
        {
            Camera.main.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            characters.transform.localPosition = Vector3.Lerp(charactersStartPos, charactersTargetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = targetPos; // Ensure the camera reaches the exact target position
        characters.transform.localPosition = charactersTargetPos;
    }

}
