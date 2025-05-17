using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShatter : MonoBehaviour
{
    public GameObject explosionPositionObject;
    public Image flashImage;
    private void Awake()
    {
        flashImage.color = new Color(1, 1, 1, 0); // Ensure transparent at start

        Camera camera = transform.Find("ShatterCamera")?.GetComponent<Camera>();
        StartCoroutine(Shatter(camera));
    }
    private IEnumerator Shatter(Camera camera)
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                 SwapAndReswap(0, 1, child.gameObject);
                 yield return null;
            }
        }

        yield return new WaitForSeconds(0.3f);

        ScreenFlash(camera);
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
    private void ScreenFlash(Camera camera)
    {
        camera!.backgroundColor = Color.white;
        StartCoroutine(ImageFlash(0.625f));
        StartCoroutine(BackgroundFlash(camera, 0.5f));
    }

    IEnumerator BackgroundFlash(Camera cam, float duration)
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
    
    public void SwapMaterials(int index1, int index2, GameObject obj)
    {
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            Debug.LogError("MeshRenderer not found.");
            return;
        }

        Material[] materials = renderer.materials;

        // Swap the two materials by index
        Material temp = materials[index1];
        materials[index1] = materials[index2];
        materials[index2] = temp;

        // Apply the changes
        renderer.materials = materials;
    }
    
    public void SwapAndReswap(int index1, int index2,  GameObject obj)
    {
        StartCoroutine(SwapAndReswapCoroutine(index1, index2, obj));
    }

    private IEnumerator SwapAndReswapCoroutine(int index1, int index2,  GameObject obj)
    {
        SwapMaterials(index1, index2, obj);

        yield return new WaitForSeconds(0.1f);

        SwapMaterials(index1, index2, obj);
    }
    
    private IEnumerator ImageFlash(float flashDuration)
    {
        Color startColor = new Color(1, 1, 1, 0);
        Color endColor = new Color(1, 1, 1, 0.5f);
        float time = 0f;

        while (time < flashDuration)
        {
            flashImage.color = Color.Lerp(startColor, endColor, time / flashDuration);
            time += Time.deltaTime;
            yield return null;
        }

        flashImage.color = endColor; // Ensure it's exactly black at the end

        time = 0f;
        
        while (time < flashDuration)
        {
            flashImage.color = Color.Lerp(endColor, startColor, time / flashDuration);
            time += Time.deltaTime;
            yield return null;
        }
        
        flashImage.color = startColor; // Ensure it's exactly black at the end

    }

  
}
