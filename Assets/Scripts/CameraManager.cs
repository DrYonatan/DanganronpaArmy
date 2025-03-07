using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager instance { get; private set; }

    public const string charactersLayerPath = "VN controller/Root/Canvas - Main/LAYERS/2 - Characters";

    private const int CHARACTERS_RIGHT = -350;
    private const int CHARACTERS_MIDDLE = 0;
    private const int CHARACTERS_LEFT = 382;

    private Vector3 initialPosition;
    private Vector3 initialCharacterPos;
    private Quaternion initialRotation;

    private void Awake()
    {
        instance = this;
        initialCharacterPos = GameObject.Find(charactersLayerPath).transform.position;
        initialRotation = Camera.main.transform.rotation;
        initialPosition = Camera.main.transform.position;
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
            z = 30;
        else
            z = 45;
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
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);

        switch (location) 
        {
           case "right" :
           x = 20;
           characterX = CHARACTERS_RIGHT;
           rotation = Quaternion.Euler(0f, 12f, 0f);
           break;

           case "middle" :
           x = 3;
           characterX = CHARACTERS_MIDDLE;
           break;

           case "left" :
           x = -20;
           characterX = CHARACTERS_LEFT;
           rotation = Quaternion.Euler(0f, -12f, 0f);
           break;
        }

        Vector3 startPos = Camera.main.transform.position;
        Vector3 targetPos = initialPosition + new Vector3(x, 0, 0);

        Quaternion startRotate = Camera.main.transform.rotation;
        Quaternion targetRotate = initialRotation * rotation;

        Vector3 charactersStartPos = characters.transform.localPosition;
        Vector3 charactersTargetPos = new Vector3(characterX, charactersStartPos.y, charactersStartPos.z);


        while (elapsedTime < duration)
        {
            Camera.main.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            Camera.main.transform.rotation = Quaternion.Slerp(startRotate, targetRotate, elapsedTime / duration);
            characters.transform.localPosition = Vector3.Lerp(charactersStartPos, charactersTargetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = targetPos; // Ensure the camera reaches the exact target position
        characters.transform.localPosition = charactersTargetPos;
    }

}
