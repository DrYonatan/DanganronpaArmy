using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; private set; }

    public Transform cameraTransform;

    public const string charactersLayerPath = "VN controller/Root/Canvas - Main/LAYERS/2 - Characters";

    private const int CHARACTERS_RIGHT = -1400;
    private const int CHARACTERS_MIDDLE = 0;
    private const int CHARACTERS_LEFT = 1400;

    public Quaternion initialRotation;

    private bool isInFinalRotation = true;
    
    private List<IEnumerator> operations = new List<IEnumerator>();

    private void Start()
    {
        instance = this;
        if(GameObject.Find("World/CameraStartPos") != null)
        initialRotation = GameObject.Find("World/CameraStartPos").transform.rotation;
    }

    public void MoveCamera(CameraLookDirection direction, float duration)
    {
        if (isInFinalRotation)
        {
            StartCameraCoroutine(StartMovingCamera(direction, duration));
        }
    }

    public void MoveCameraTo(Transform location)
    {
        float duration = 0.5f;
        initialRotation = location.rotation;
        StartCameraCoroutine(MoveCameraTo(location.position, duration));
        StartCameraCoroutine(RotateCameraTo(location.rotation, duration));
    }

    public void ReturnToDollyTrack()
    {
        StartCameraCoroutine(MoveCameraToDollyTrack());
    }

    private IEnumerator MoveCameraToDollyTrack()
    {
        float duration = 0.5f;
        Vector3 vCamPosition = VirutalCameraManager.instance.virtualCamera.State.FinalPosition;
        Quaternion vCamRotation = VirutalCameraManager.instance.virtualCamera.State.FinalOrientation;
        initialRotation = vCamRotation;
        StartCameraCoroutine(RotateCameraTo(vCamRotation, duration));
        yield return StartCameraCoroutine(MoveCameraTo(vCamPosition, duration));
        if (!DialogueSystem.instance.isActive)
            VirutalCameraManager.instance.EnableVirtualCamera();
    }

    public void ZoomCamera(string zoom)
    {
        StartCameraCoroutine(Zoom(zoom));
    }

    public IEnumerator RotateCameraTo(Quaternion rotation, float duration)
    {
        isInFinalRotation = false;

        Quaternion startRotate = Camera.main.transform.rotation;

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            if (rotation != null)
            {
                Camera.main.transform.rotation = Quaternion.Slerp(startRotate, rotation, elapsedTime / duration);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (rotation != null)
        {
            Camera.main.transform.rotation = rotation;
        }

        isInFinalRotation = true;
    }

    public IEnumerator MoveCameraTo(Vector3 location, float duration)
    {
        Vector3 startPos = Camera.main.transform.position;

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            if (location != null)
            {
                Camera.main.transform.position = Vector3.Lerp(startPos, location, elapsedTime / duration);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (location != null)
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


    IEnumerator StartMovingCamera(CameraLookDirection direction,  float duration)
    {
        GameObject characters = GameObject.Find(charactersLayerPath);
        float elapsedTime = 0;
        int characterX = 0;
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);

        switch (direction)
        {
            case CameraLookDirection.Right:
                characterX = CHARACTERS_RIGHT;
                rotation = Quaternion.Euler(0f, 12f, 0f);
                break;

            case CameraLookDirection.Middle:
                characterX = CHARACTERS_MIDDLE;
                break;

            case CameraLookDirection.Left:
                characterX = CHARACTERS_LEFT;
                rotation = Quaternion.Euler(0f, -12f, 0f);
                break;
        }

        Quaternion startRotate = Camera.main.transform.rotation;
        Quaternion targetRotate = initialRotation * rotation;

        Vector3 charactersStartPos = characters.transform.localPosition;
        Vector3 charactersTargetPos = new Vector3(characterX, charactersStartPos.y, charactersStartPos.z);


        while (elapsedTime < duration)
        {
            Camera.main.transform.rotation = Quaternion.Slerp(startRotate, targetRotate, elapsedTime / duration);
            characters.transform.localPosition =
                Vector3.Lerp(charactersStartPos, charactersTargetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        characters.transform.localPosition = charactersTargetPos;
    }

    public void ChangeCameraBackground(bool isInside)
    {
        Camera.main.clearFlags = isInside ? CameraClearFlags.SolidColor : CameraClearFlags.Skybox;
    }
    public Coroutine StartCameraCoroutine(IEnumerator operation)
    {
        Coroutine coroutine = StartCoroutine(operation);
        operations.Add(operation);
        return coroutine;
    }

    public void StopAllPreviousOperations()
    {
        foreach (IEnumerator operation in operations)
        {
            StopCoroutine(operation);
        }
    }
}