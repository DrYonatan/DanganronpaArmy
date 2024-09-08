using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public void MoveCamera(string direction, float duration)
    {
        StartCoroutine(MoveCamera(duration, direction));

    }

    public void ZoomCamera(string zoom)
    {
        StartCoroutine(Zoom(zoom));
    }

    IEnumerator Zoom(string zoom)
    {
        float z = 0;
        float elapsedTime = 0;
        if (zoom.ToLower() == "in")
            z = 54;
        else
            z = 85;
        while (Camera.main.fieldOfView != z)
        {
            Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, z, 60 * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.fieldOfView = z;




    }  

    IEnumerator MoveCamera(float duration, string location)
    {
        float elapsedTime = 0;
        int x = 0;
        if (location == "right")
            x = 2363;
        else if (location == "middle")
            x = 963;
        else if (location == "left")
            x = -437;
        Vector3 startPos = Camera.main.transform.position;
        Vector3 targetPos = new Vector3(x, 559, -984);


        while (elapsedTime < duration)
        {
            Camera.main.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = targetPos; // Ensure the camera reaches the exact target position
    }

}
