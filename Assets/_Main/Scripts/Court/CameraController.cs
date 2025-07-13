using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform pivot;

    public Transform cameraTransform;
    public Camera camera;
    CameraEffectController effectController;

    [SerializeField] float newAngle, rotationTime, radius;
    [SerializeField] float speed = 50f;
    float height;


    private Coroutine spinCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        pivot = cameraTransform.parent;
        effectController = GetComponent<CameraEffectController>();
        height = cameraTransform.position.y;
    }

    public void TeleportToTarget(Transform target)
    {
        Vector3 targetDir = target.forward;
        Vector3 targetPosition = target.position - target.forward * 10f;
        targetPosition.y += 1f;
        cameraTransform.position = targetPosition;
        cameraTransform.rotation = Quaternion.LookRotation(targetDir);
    }

    public IEnumerator SpinToTarget(Transform target, Vector3 positionOffset, Vector3 rotationOffset, float fovOffset)
    {
        if (spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);
        }
        Vector3 newPosition = target.position + target.right * positionOffset.x
                                              + target.up * (positionOffset.y + 0.75f)
                                              + target.forward * positionOffset.z;
        Quaternion newRotation = Quaternion.Euler(rotationOffset);
        float newFOV = 15 + fovOffset;


        spinCoroutine = StartCoroutine(SpinningToTarget(newPosition, newRotation, newFOV, rotationTime));
        yield return spinCoroutine;
    }
    public IEnumerator SpinningToTarget(Vector3 targetPosition, Quaternion targetRotation, float targetFov, float duration)
    {
        // --- Direction & radius setup ---
        Vector3 directionToTarget = targetPosition - pivot.position;
        Vector3 desiredDirection = -directionToTarget.normalized;
        
        // Start and end angles in radians
        Vector3 currentDirection = (transform.position - pivot.position).normalized;
        float startAngle = Mathf.Atan2(currentDirection.z, currentDirection.x);
        float endAngle = Mathf.Atan2(desiredDirection.z, desiredDirection.x);

        // Angle difference in degrees (shortest path)
        float angleDiff = Mathf.DeltaAngle(startAngle * Mathf.Rad2Deg, endAngle * Mathf.Rad2Deg);

        // Y transition
        float startY = cameraTransform.position.y;
        float endY = targetPosition.y;
        Quaternion startRotation = cameraTransform.rotation;
        float startFov = camera.fieldOfView;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // Interpolated angle (in radians)
            float angle = startAngle + Mathf.Deg2Rad * angleDiff * t;

            // Interpolated Y
            float y = Mathf.Lerp(startY, endY, t);

            // New position along the circle at height y
            Vector3 newPos = pivot.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            newPos.y = y;

            cameraTransform.position = newPos;
            Vector3 targetDirection = pivot.position - cameraTransform.position;
            targetDirection.y = 0;
            Quaternion offSetRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            offSetRotation.y = 0;
            cameraTransform.rotation = Quaternion.LookRotation(targetDirection) * offSetRotation;
            camera.fieldOfView = Mathf.Lerp(startFov, targetFov, t);

            yield return null;
        }

        // Final snap
        Vector3 finalPosition = pivot.position + desiredDirection * radius;
        finalPosition.y = endY;
        cameraTransform.position = finalPosition;
        Vector3 finalDirection = pivot.position - cameraTransform.position;
        finalDirection.y = 0;
        cameraTransform.rotation = Quaternion.LookRotation(finalDirection) * targetRotation;
    }
    public IEnumerator MoveCameraOnXAndZ(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 startPos = cameraTransform.position;
        Quaternion startRotation = cameraTransform.rotation;
        targetPosition.y = startPos.y;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            cameraTransform.position = Vector3.Lerp(startPos, targetPosition, elapsedTime / duration);
            cameraTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / (duration * 3f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.position = targetPosition;
    }
}