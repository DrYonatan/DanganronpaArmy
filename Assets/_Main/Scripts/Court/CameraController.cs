using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform pivot;

    public Transform cameraTransform;
    CameraEffectController effectController;

    [SerializeField] float newAngle, rotationTime, radius;
    [SerializeField] float speed = 50f;
    float height;


    private Coroutine spinCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        pivot = transform.parent;
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

    public void SpinToTarget(Transform target)
    {
        if (spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);
        }

        spinCoroutine = StartCoroutine(SpinningToTarget(target));
    }
    IEnumerator SpinningToTarget(Transform target)
    {
        float heightOffset = target.position.y - 2f;

        // Start angle
        Vector3 toCamera = transform.position - pivot.position;
        toCamera.y = 0f;
        float startAngle = Mathf.Atan2(toCamera.z, toCamera.x) * Mathf.Rad2Deg;

        // Opposite of target angle
        Vector3 toTarget = target.position - pivot.position;
        toTarget.y = 0f;
        float targetAngle = Mathf.Atan2(toTarget.z, toTarget.x) * Mathf.Rad2Deg;
        float oppositeAngle = (targetAngle + 180f) % 360f;

        float angleDelta = Mathf.DeltaAngle(startAngle, oppositeAngle);
        float direction = Mathf.Sign(angleDelta);
        float totalRotation = Mathf.Abs(angleDelta);
        float rotated = 0f;
        float currentAngle = startAngle;

        while (rotated < totalRotation)
        {
            float step = speed * Time.deltaTime;
            float remaining = totalRotation - rotated;
            float actualStep = Mathf.Min(step, remaining);
            currentAngle += actualStep * direction;
            rotated += actualStep;

            float rad = currentAngle * Mathf.Deg2Rad;
            Vector3 newPos = new Vector3(
                Mathf.Cos(rad) * radius,
                heightOffset,
                Mathf.Sin(rad) * radius
            );
            transform.position = pivot.position + newPos;

            // Look at pivot, ignore X rotation
            Vector3 lookDir = pivot.position - transform.position;
            lookDir.y = 0f;
            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion rot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Euler(0f, rot.eulerAngles.y, 0f);
            }

            yield return null;
        }
    }

    // IEnumerator SpinningToTarget(Transform target)
    // {
    //     float elapsedTime = 0f;
    //
    //     while (elapsedTime < rotationTime)
    //     {
    //         elapsedTime += Time.deltaTime;
    //
    //         // Flatten positions to ground plane
    //         Vector3 pivotPos = pivot.position;
    //         Vector3 targetPos = target.position;
    //
    //         pivotPos.y = 0;
    //         targetPos.y = 0;
    //
    //         // Direction from pivot to target
    //         Vector3 dirToTarget = (targetPos - pivotPos).normalized;
    //
    //         // Angle from pivot forward to target direction
    //         float angleToTarget = Vector3.SignedAngle(Vector3.forward, dirToTarget, Vector3.up);
    //
    //         // Opposite angle (add 180 degrees)
    //         float desiredAngle = angleToTarget + 180f;
    //
    //         // Smoothly move angle over time
    //         float currentAngle = Mathf.LerpAngle(0f, desiredAngle, elapsedTime / rotationTime);
    //
    //         // Convert angle to radians for position calculation
    //         float rad = currentAngle * Mathf.Deg2Rad;
    //
    //         // Calculate new position around pivot at given radius
    //         float x = Mathf.Sin(rad) * radius;
    //         float z = Mathf.Cos(rad) * radius;
    //
    //         Vector3 newPos = new Vector3(x, 0, z) + pivot.position;
    //
    //         transform.position = newPos;
    //
    //         // Look at the pivot
    //         transform.rotation = Quaternion.LookRotation(pivot.position - transform.position);
    //
    //         yield return null;
    //     }
    //
    //     spinCoroutine = null;
    // }

    public IEnumerator MoveCameraOnXAndZ(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Transform cameraTransform = Camera.main.transform;
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