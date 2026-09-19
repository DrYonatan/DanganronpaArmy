using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using DG.Tweening;
using UnityEngine;
using DIALOGUE;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; private set; }

    public CharacterController player;
    public Transform cameraTransform;
    
    public Quaternion initialRotation;

    public bool isInFinalRotation = true;

    public bool conversationFinishedMoving = true;

    public AudioClip footStepsSound;
    public AudioClip fastFootStepsSound;

    public AudioSource footStepsSource;

    private List<IEnumerator> operations = new List<IEnumerator>();

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameStateManager.instance.sceneTransitionCamera.gameObject.SetActive(false);
    }

    public IEnumerator MoveCamera(CameraLookDirection direction, float duration)
    {
        if(isInFinalRotation)
           yield return StartCameraCoroutine(StartMovingCamera(direction, duration));
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

    public IEnumerator RotateCameraTo(Quaternion rotation, float duration)
    {
        isInFinalRotation = false;

        Quaternion startRotate = cameraTransform.rotation;

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            cameraTransform.rotation = Quaternion.Slerp(startRotate, rotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.rotation = rotation;

        isInFinalRotation = true;
    }

    public IEnumerator MoveCameraTo(Vector3 location, float duration, Ease ease = Ease.OutQuad)
    {
        cameraTransform.DOMove(location, duration).SetEase(ease);
        yield return new WaitForSeconds(duration);

        cameraTransform.position = location; // Ensure the camera reaches the exact target position
    }

    private IEnumerator StartMovingCamera(CameraLookDirection direction, float duration)
    {
        conversationFinishedMoving = false;
        RectTransform charactersLayer = VNCharacterManager.instance.characterLayer;
        float elapsedTime = 0;
        float characterX = 0;
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);

        switch (direction)
        {
            case CameraLookDirection.Right:
                characterX = -VNCharacterManager.instance.right.anchoredPosition.x;
                rotation = Quaternion.Euler(0f, 30f, 0f);
                break;

            case CameraLookDirection.MidRight:
                characterX = -VNCharacterManager.instance.midRight.anchoredPosition.x;
                rotation = Quaternion.Euler(0f, 15f, 0f);
                break;

            case CameraLookDirection.Middle:
                characterX = -VNCharacterManager.instance.middle.anchoredPosition.x;
                break;

            case CameraLookDirection.MidLeft:
                characterX = -VNCharacterManager.instance.midLeft.anchoredPosition.x;
                rotation = Quaternion.Euler(0f, -15f, 0f);
                break;

            case CameraLookDirection.Left:
                characterX = -VNCharacterManager.instance.left.anchoredPosition.x;
                rotation = Quaternion.Euler(0f, -30f, 0f);
                break;
        }

        Quaternion startRotate = cameraTransform.rotation;
        Quaternion targetRotate = initialRotation * rotation;

        Vector2 charactersStartPos = charactersLayer.anchoredPosition;
        Vector2 charactersTargetPos = new Vector2(characterX, charactersStartPos.y);


        if (startRotate != targetRotate)
        {
            while (elapsedTime < duration)
            {
                cameraTransform.rotation = Quaternion.Slerp(startRotate, targetRotate, elapsedTime / duration);
                charactersLayer.anchoredPosition =
                    Vector3.Lerp(charactersStartPos, charactersTargetPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        cameraTransform.rotation = targetRotate;
        charactersLayer.anchoredPosition = charactersTargetPos;
        conversationFinishedMoving = true;
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