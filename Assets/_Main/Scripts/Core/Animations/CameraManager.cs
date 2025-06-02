using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class CameraManager : MonoBehaviour
{

    public static CameraManager instance { get; private set; }

    public const string charactersLayerPath = "VN controller/Root/Canvas - Main/LAYERS/2 - Characters";

    private const int CHARACTERS_RIGHT = -1400;
    private const int CHARACTERS_MIDDLE = 0;
    private const int CHARACTERS_LEFT = 1400;

    private Vector3 initialPosition;
    private Vector3 initialCharacterPos;
    private Quaternion initialRotation;

    private void Start()
    {
        instance = this;
        initialCharacterPos = GameObject.Find(charactersLayerPath).transform.position;
        Transform absoluteStartPos = GameObject.Find("World/CameraStartPos").transform;
        setInitialPosition(absoluteStartPos.position, absoluteStartPos.rotation);
    }

    public void setInitialPosition(Vector3 position, Quaternion rotation) 
    {
        initialPosition = position;
        initialRotation = rotation;
    }

    public void MoveCamera(string direction, float duration)
    {
        StartCoroutine(MoveCamera(duration, direction));

    }

    public void MoveCameraTo(Transform location) 
    {
        float duration = 0.5f;
        setInitialPosition(location.position, location.rotation);
        StartCoroutine(MoveCameraTo(location.position, duration));
        StartCoroutine(RotateCameraTo(location.rotation, duration));
    }

    public void ReturnToDollyTrack()
    {
        StartCoroutine(MoveCameraToDollyTrack());
    }

    public IEnumerator MoveCameraToDollyTrack()
    {
        float duration = 0.5f;
        Vector3 vCamPosition = VirutalCameraManager.instance.virtualCamera.State.FinalPosition;
        Quaternion vCamRotation = VirutalCameraManager.instance.virtualCamera.State.FinalOrientation;
        setInitialPosition(vCamPosition, vCamRotation);
        StartCoroutine(RotateCameraTo(vCamRotation, duration));
        yield return MoveCameraTo(vCamPosition, duration);
        if(!DialogueSystem.instance.isActive)
        VirutalCameraManager.instance.EnableVirtualCamera();
    }

    public void ZoomCamera(string zoom)
    {
        StartCoroutine(Zoom(zoom));
    }

    public IEnumerator RotateCameraTo(Quaternion rotation, float duration)
    {
        Quaternion startRotate = Camera.main.transform.rotation;
        
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            if(rotation != null)
            {
               Camera.main.transform.rotation = Quaternion.Slerp(startRotate, rotation, elapsedTime / duration);
            }
          
            elapsedTime += Time.deltaTime;
            yield return null;
        }

         if(rotation != null)
        {
           Camera.main.transform.rotation = rotation;
        }
    }

    public IEnumerator MoveCameraTo(Vector3 location, float duration) 
    {
        Vector3 startPos = Camera.main.transform.position;
        
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            if(location != null)
            {
               Camera.main.transform.position = Vector3.Lerp(startPos, location, elapsedTime / duration);
            }
          
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if(location != null)
        {
           Camera.main.transform.position = location; // Ensure the camera reaches the exact target position
        }
        
    }


    IEnumerator Zoom(string zoom)
    {
        float z = 0;
        float elapsedTime = 0;
        if (zoom.ToLower() == "in")
            z = 30;
        else
            z = 60;
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
        int characterX = 0;
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);

        switch (location) 
        {
           case "right" :
           characterX = CHARACTERS_RIGHT;
           rotation = Quaternion.Euler(0f, 12f, 0f);
           break;

           case "middle" :
           characterX = CHARACTERS_MIDDLE;
           break;

           case "left" :
           characterX = CHARACTERS_LEFT;
           rotation = Quaternion.Euler(0f, -12f, 0f);
           break;
        }

        Vector3 startPos = Camera.main.transform.position;

        Quaternion startRotate = Camera.main.transform.rotation;
        Quaternion targetRotate = initialRotation * rotation;

        Vector3 charactersStartPos = characters.transform.localPosition;
        Vector3 charactersTargetPos = new Vector3(characterX, charactersStartPos.y, charactersStartPos.z);


        while (elapsedTime < duration)
        {
            Camera.main.transform.rotation = Quaternion.Slerp(startRotate, targetRotate, elapsedTime / duration);
            characters.transform.localPosition = Vector3.Lerp(charactersStartPos, charactersTargetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        characters.transform.localPosition = charactersTargetPos;
    }

}
