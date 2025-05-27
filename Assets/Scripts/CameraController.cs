using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    Transform pivot;

    [SerializeField] Transform cameraTransform;
    CameraEffectController effectController;

    [SerializeField] float newAngle, rotationTime, radius;
    [SerializeField] float speed = 50f;
    float height;

    public GameObject noThatsWrong;


    // Start is called before the first frame update
    void Start()
    {
        pivot = transform.parent;
        effectController = GetComponent<CameraEffectController>();
        height = cameraTransform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameLoop.instance.finished)
        {
            Vector3 targetDir = target.position - pivot.position;
            targetDir.y = 0;
            Vector3 cameraDir = cameraTransform.position - effectController.position - pivot.position;
            cameraDir.y = 0;

            float targetAngle = Vector3.SignedAngle(targetDir, pivot.forward, Vector3.up);
            float cameraAngle = Vector3.SignedAngle(cameraDir, pivot.forward, Vector3.up);

            //speed = (Mathf.PI * 2)/rotationTime;
            //newAngle -= speed * Time.deltaTime;
            newAngle = Mathf.MoveTowardsAngle(cameraAngle, targetAngle, speed * Time.deltaTime);
            newAngle *= Mathf.Deg2Rad;
            float x = Mathf.Sin(-newAngle) * (radius + effectController.zoom);
            float z = Mathf.Cos(newAngle) * (radius + effectController.zoom);

            Vector3 cameraPos = cameraTransform.position;
            Vector3 textPivotPos = pivot.position;

            cameraPos.y = 0f;
            textPivotPos.y = 0f;

            transform.position = new Vector3(x, height, z) + effectController.position;

            //look away from the pivot
            transform.rotation = Quaternion.LookRotation(transform.position - pivot.position);
            cameraTransform.rotation =
                Quaternion.Euler(cameraTransform.rotation.eulerAngles + effectController.rotation);
        }
    }

    public void OnHitStatement()
    {
        StartCoroutine(DebateHitEffect());
    }

    IEnumerator DebateHitEffect()
    {
        Transform cameraTransform = Camera.main.transform;
        Vector3 startPos = cameraTransform.position;
        Vector3 forwardLocation = -cameraTransform.forward;
        Vector3 targetPosition =  startPos + forwardLocation;
        Quaternion targetRotation = cameraTransform.rotation * Quaternion.Euler(0f, 15f, 0f);
        Quaternion oppositeRotation = cameraTransform.rotation * Quaternion.Euler(0f, -5f, 0f);
        StartCoroutine(PlayNoThatsWrong(1.5f));
        yield return MoveCameraOnXAndZ(targetPosition, targetRotation, 0.2f);
        yield return MoveCameraOnXAndZ(startPos - forwardLocation, targetRotation, 0.2f);
        yield return MoveCameraOnXAndZ(targetPosition, oppositeRotation, 4f);
        

    }

    IEnumerator PlayNoThatsWrong(float delay)
    {
        yield return new WaitForSeconds(delay);

        noThatsWrong.SetActive(true);
        SoundManager.instance.PlaySoundEffect("nothatswrong");

        yield return new WaitForSeconds(3f);

        noThatsWrong.SetActive(false);
    }

    IEnumerator MoveCameraOnXAndZ(Vector3 targetPosition, Quaternion targetRotation, float duration)
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