using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform pivot;

    public Transform cameraTransform;
    public Camera camera;
    CameraEffectController effectController;

    [SerializeField] float newAngle, rotationTime, radius;
    [SerializeField] float speed = 50f;
    private Vector3 cameraDefaultLocalPosition;


    private Coroutine spinCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        pivot = cameraTransform.parent;
        effectController = GetComponent<CameraEffectController>();
        cameraDefaultLocalPosition = cameraTransform.localPosition;
    }

    public void TeleportToTarget(Transform target)
    {
        Vector3 targetDir = target.forward;
        Vector3 targetPosition = target.position - target.forward * 10f;
        targetPosition.y += 1f;
        cameraTransform.position = targetPosition;
        cameraTransform.rotation = Quaternion.LookRotation(targetDir);
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

    public IEnumerator MoveCameraOnXAndZ(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 startPos = cameraTransform.localPosition;
        Quaternion startRotation = cameraTransform.localRotation;
        targetPosition.y = startPos.y;
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
}