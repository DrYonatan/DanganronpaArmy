using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandEffect : MonoBehaviour
{
    public float delay = 0f;
    public float duration = 1f;
    public Vector3 degreesOffset = new Vector3(-90f, 0f, 0f);
    public GameObject parentObject;

    private Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {   
        CreateParentGameObject();
        targetRotation = parentObject.transform.rotation;
        parentObject.transform.Rotate(degreesOffset);
        StartCoroutine(PlayEffect());
    }

    private void CreateParentGameObject()
    {
        float positionY = transform.GetComponent<Renderer>().bounds.min.y;
        Vector3 parentPosition = new Vector3(transform.position.x, positionY, transform.position.z);
        parentObject = new GameObject(gameObject.name + " Parent");
        parentObject.transform.position = parentPosition;
        transform.SetParent(parentObject.transform);
    }

    private IEnumerator PlayEffect()
    {
        yield return new WaitForSeconds(delay);
        float elapsedTime = 0f;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            parentObject.transform.rotation = Quaternion.Slerp(parentObject.transform.rotation, targetRotation, elapsedTime / duration);
            yield return null;
        }
        
        parentObject.transform.rotation = targetRotation;
    }
}
