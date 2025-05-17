using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShatter : MonoBehaviour
{
    public GameObject explosionPositionObject;
    private void Awake()
    {

        ScreenFlash();
        Vector3 explosionPosition = explosionPositionObject.transform.position;
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                float direction = Mathf.Sign(explosionPosition.x - child.position.x);
                childRigidbody.AddExplosionForce(17f + direction * 8f, explosionPosition, 10f);
                childRigidbody.AddTorque(0, 0, 0.2f + 0.3f * direction, ForceMode.Impulse);
            }
        }
    }

    private void ScreenFlash()
    {
        Camera camera = transform.Find("ShatterCamera")?.GetComponent<Camera>();
        camera!.backgroundColor = Color.white;
        StartCoroutine(Flash(camera, 0.5f));
    }

    IEnumerator Flash(Camera cam, float duration)
    {
        yield return new WaitForSeconds(0.75f);
        Color startColor = cam.backgroundColor;
        Color endColor = Color.black;
        float time = 0f;

        while (time < duration)
        {
            cam.backgroundColor = Color.Lerp(startColor, endColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        cam.backgroundColor = endColor; // Ensure it's exactly black at the end
    }
}
