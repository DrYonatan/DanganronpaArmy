using UnityEngine;
using System.Collections;

public class QuestionMarkShooter : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject questionMarkPrefab;
    public float travelTime = 1.2f;
    public float curveAmount = 1.0f; // How far it veers side-to-side

    public IEnumerator ShootQuestionMark(Vector3 target)
    {
        // Start point is slightly in front of the camera
        Vector3 start = mainCamera.transform.position + mainCamera.transform.forward * 0.5f;

        // Choose control points for a wavy arc
        Vector3 mid = Vector3.Lerp(start, target, 0.5f);
        Vector3 right = Vector3.Cross((target - start).normalized, Vector3.up);
        mid += right * Random.Range(-curveAmount, curveAmount); // Add side offset to curve

        GameObject qm = Instantiate(questionMarkPrefab, start, mainCamera.transform.rotation);
        float elapsed = 0;

        while (elapsed < travelTime)
        {
            float t = elapsed / travelTime;

            // Bezier-style interpolation
            Vector3 p1 = Vector3.Lerp(start, mid, t);
            Vector3 p2 = Vector3.Lerp(mid, target, t);
            qm.transform.position = Vector3.Lerp(p1, p2, t);
            qm.transform.Rotate(Vector3.forward * 360f * Time.deltaTime);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        qm.transform.position = target;
        // Optional: destroy or animate on hit
        Destroy(qm);
    }
}
