using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandEffect : MonoBehaviour
{
    public float delay = 0f;
    public float duration = 1f;
    public Vector3 degreesOffset = new Vector3(0f, 0f, -90f);

    private Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        targetRotation = transform.rotation;
        transform.Rotate(degreesOffset);
        StartCoroutine(PlayEffect());
    }

    private IEnumerator PlayEffect()
    {
        yield return new WaitForSeconds(delay);
        float elapsedTime = 0f;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime / duration);
            yield return null;
        }
        
        transform.rotation = targetRotation;
    }
}
