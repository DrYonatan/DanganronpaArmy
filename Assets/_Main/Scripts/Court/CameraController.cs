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

    public void SpinToTarget(Transform target, Vector3 positionOffset, Vector3 rotationOffset, float fovOffset)
    {
        if (spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);
        }
        Vector3 newPosition = target.right * positionOffset.x
                              + target.up * positionOffset.y
                              + target.forward * positionOffset.z;
        Quaternion newRotation = cameraTransform.rotation * Quaternion.Euler(rotationOffset);
        float newFOV = 15 + fovOffset;


        spinCoroutine = StartCoroutine(SpinningToTarget(target.position + newPosition));
    }
    IEnumerator SpinningToTarget(Vector3 target)
    {
        float heightOffset = target.y - 2f;

        // Start angle
        Vector3 toCamera = transform.position - pivot.position;
        toCamera.y = 0f;
        float startAngle = Mathf.Atan2(toCamera.z, toCamera.x) * Mathf.Rad2Deg;

        // Opposite of target angle
        Vector3 toTarget = target - pivot.position;
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