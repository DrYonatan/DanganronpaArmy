using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DIALOGUE;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance { get; private set; }

    public Transform pivot;

    public Transform cameraTransform;
    public Camera camera;
    public Camera uiCamera;

    [SerializeField] float newAngle, rotationTime, radius;
    [SerializeField] float speed = 50f;
    private Vector3 cameraDefaultLocalPosition;


    private Coroutine spinCoroutine;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pivot = cameraTransform.parent;
        cameraDefaultLocalPosition = new Vector3(0f, 0f, -3f);
        GameStateManager.instance.sceneTransitionCamera.gameObject.SetActive(false);
    }

    public IEnumerator DiscussionOutroMovement(float duration)
    {
        float elapsedTime = 0f;
        ImageScript.instance.FadeToBlack(2f);
        Vector3 startPos = cameraTransform.localPosition;
        Quaternion startRotation = cameraTransform.localRotation;
        float startFOV = camera.fieldOfView;
        while (elapsedTime < duration)
        {
            camera.fieldOfView = Mathf.Lerp(startFOV, 30f, elapsedTime / duration);
            cameraTransform.localPosition = Vector3.Lerp(startPos, cameraDefaultLocalPosition + Vector3.up * 3.5f, elapsedTime / duration);
            cameraTransform.localRotation = Quaternion.Slerp(startRotation, Quaternion.Euler(new Vector3(0f, 0f, 0f)), elapsedTime / duration);
            pivot.Rotate(Vector3.up, 120f * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cameraTransform.localPosition = cameraDefaultLocalPosition;
    }

    public IEnumerator DiscussionIntroMovement(float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPos = new Vector3(0f, 8f, -18f);
        pivot.rotation = new Quaternion(0f, 0f, 0f, 0f);
        cameraTransform.localRotation = Quaternion.Euler(new Vector3(15f, 0, 0));
        camera.fieldOfView = 30f;
        bool uiAppeared = false;
        while (elapsedTime < duration)
        {
            if (elapsedTime > duration / 2 && !uiAppeared)
            {
                DialogueSystem.instance.dialogueBoxAnimator.TextBoxAppear();
                uiAppeared = true;
            }
            cameraTransform.localPosition = Vector3.Lerp(startPos, startPos + cameraTransform.forward * 4f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator DebateStartCameraMovement(float duration)
    {
        TrialManager.instance.barsAnimator.HideDebateBars(0f);

        yield return DescendingCircling(duration);
        
        cameraTransform.localPosition = cameraDefaultLocalPosition + Vector3.up * 4f;
        GameLoop.instance.debateUIAnimator.OpenBulletSelectionMenu();
        GameLoop.instance.LoadBullets();
        cameraTransform.localRotation = Quaternion.Euler(0f, 0f, -5f);
        camera.fieldOfView = 15f;
        float elapsedTime = 0f;
        bool triggeredUI = false;
        while (elapsedTime < duration * 2.5f)
        {
            pivot.Rotate(Vector3.up, Time.deltaTime * -20f);
            elapsedTime += Time.deltaTime;
            if (!triggeredUI && elapsedTime >= (duration * 2.5f - 1f))
            {
                Sequence sequence = DOTween.Sequence();
                sequence.AppendCallback(() => GameLoop.instance.debateUIAnimator.CloseBulletSelectionMenu());
                sequence.AppendInterval(0.5f); // optional short delay between them
                sequence.AppendCallback(() => GameLoop.instance.debateUIAnimator.DebateUIAppear());
                triggeredUI = true;
            }
            yield return null;
        }
        
    }

    public IEnumerator DescendingCircling(float duration)
    {
        camera.fieldOfView = 30f;
        
        cameraTransform.localRotation *= Quaternion.Euler(new Vector3(10f, 0f, 10f));
        
        Vector3 cameraStartPos = cameraDefaultLocalPosition + new Vector3(0f, 8f, -20f);
        cameraTransform.localPosition = cameraStartPos;
        
        Quaternion cameraStartRot = Quaternion.Euler(new Vector3(15, 0f, 0f));
        cameraTransform.localRotation = cameraStartRot;
        
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            pivot.Rotate(Vector3.up, Time.deltaTime * -90f);
            cameraTransform.localPosition = Vector3.Lerp(cameraStartPos, cameraDefaultLocalPosition + Vector3.up * 3f, elapsedTime  / duration);
           
            cameraTransform.localRotation = Quaternion.Lerp(cameraStartRot, Quaternion.Euler(0f, 0f, 0f), elapsedTime / duration);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void TeleportToTarget(Transform target, Transform heightPivot, Vector3 positionOffset, Vector3 rotationOffset, float fovOffset)
    {
        pivot.rotation = Quaternion.LookRotation(new Vector3(target.position.x, 0, target.position.z));
        cameraTransform.localPosition = positionOffset + new Vector3(0, heightPivot.position.y, -1.65f);
        cameraTransform.localRotation = Quaternion.Euler(rotationOffset);
        cameraTransform.GetComponent<Camera>().fieldOfView = 15 + fovOffset;
    }

    public IEnumerator SpinToTarget(Transform target, Transform heightPivot, Vector3 positionOffset,
        Vector3 rotationOffset,
        float fovOffset)
    {
        if (spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);
        }

        spinCoroutine = StartCoroutine(SpinningToTarget(target.position,
            positionOffset + new Vector3(0, heightPivot.position.y, 0),
            Quaternion.Euler(rotationOffset), 15 + fovOffset, rotationTime));
        yield return spinCoroutine;
    }

    public IEnumerator SpinningToTarget(Vector3 targetPosition, Vector3 positionOffset, Quaternion rotationOffset,
        float fovOffset,
        float rotationTime)
    {
        float elapedTime = 0f;
        Quaternion start = pivot.rotation;
        Vector3 lookDirection = targetPosition - pivot.position;
        lookDirection.y = 0;
        Quaternion targetDirection = Quaternion.LookRotation(lookDirection);
        float startFOV = camera.fieldOfView;
        Vector3 startPos = cameraTransform.localPosition;
        Quaternion startRotation = cameraTransform.localRotation;

        while (elapedTime < rotationTime)
        {
            pivot.rotation = Quaternion.Slerp(start, targetDirection, elapedTime / rotationTime);
            camera.fieldOfView = Mathf.Lerp(startFOV, fovOffset, elapedTime / rotationTime);
            cameraTransform.localPosition = Vector3.Lerp(startPos,
                cameraDefaultLocalPosition + positionOffset, elapedTime / rotationTime);
            cameraTransform.localRotation = Quaternion.Slerp(startRotation, rotationOffset, elapedTime / rotationTime);
            elapedTime += Time.deltaTime;
            yield return null;
        }

        pivot.rotation = targetDirection;
        cameraTransform.localPosition = cameraDefaultLocalPosition + positionOffset;
        cameraTransform.localRotation = rotationOffset;
        camera.fieldOfView = fovOffset;
    }

    public IEnumerator ChangeFov(float start, float target, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            camera.fieldOfView = Mathf.Lerp(start, target, elapsed / duration);
            yield return null;
        }
    }

    public IEnumerator MoveCamera(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 startPos = cameraTransform.localPosition;
        Quaternion startRotation = cameraTransform.localRotation;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            cameraTransform.localPosition = Vector3.Lerp(startPos, targetPosition, elapsedTime / duration);
            cameraTransform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = targetPosition;
        cameraTransform.localRotation = targetRotation;
    }

    private IEnumerator ChangeFov(float targetFov, float duration)
    {
        float elapsedTime = 0;
        float startFov = camera.fieldOfView;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            camera.fieldOfView = Mathf.Lerp(startFov, targetFov, elapsedTime / duration);
            yield return null;
        }
    }
    public IEnumerator MoveAndRotate(Vector3 position, Vector3 rotation, float duration)
    {
        float elapsedTime = 0;
        Vector3 startPos = cameraTransform.localPosition;
        Quaternion startRotation = cameraTransform.localRotation;
        Vector3 targetPos = startPos + position;
        Quaternion targetRotation = startRotation * Quaternion.Euler(rotation);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            cameraTransform.localPosition = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            cameraTransform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            yield return null;
        }
    }

    public IEnumerator FovOutro()
    {
        cameraTransform.localPosition = new Vector3(0, 4, -4.7f);
        StartCoroutine(ChangeFov(25f, 1.5f));
        StartCoroutine(MoveAndRotate(new Vector3(0f, 0f, 2f), new Vector3(0f, 0f, 0f), 1.5f));
        yield return new WaitForSeconds(1f);
        DialogueSystem.instance.dialogueBoxAnimator.TextBoxAppear();
        yield return new WaitForSeconds(0.5f);
    }
}