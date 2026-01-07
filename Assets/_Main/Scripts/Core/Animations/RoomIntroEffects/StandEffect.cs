using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandEffect : RoomIntroEffect
{
    public float delay = 0f;
    public float duration = 1f;
    public Vector3 degreesOffset = new Vector3(-90f, 0f, 0f);
    public GameObject parentObject;
    
    private Renderer renderer;
    private Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {   
        renderer = GetComponent<Renderer>();
        CreateParentGameObject();
    }

    private void CreateParentGameObject()
    {
        float positionY = transform.GetComponent<Renderer>().bounds.min.y;
        Vector3 parentPosition = new Vector3(transform.position.x, positionY, transform.position.z);
        parentObject = new GameObject(gameObject.name + " Parent");
        parentObject.transform.SetParent(transform.parent);
        parentObject.transform.position = parentPosition;
        transform.SetParent(parentObject.transform);
    }

    public override IEnumerator PlayEffect()
    {
        targetRotation = parentObject.transform.rotation;
        parentObject.transform.Rotate(degreesOffset);
        
        renderer.enabled = false;
        yield return new WaitForSeconds(delay);
        renderer.enabled = true;
        float elapsedTime = 0f;

        while(elapsedTime < duration)
        {
            if (parentObject == null)
                yield break;
            
            elapsedTime += Time.deltaTime;
            parentObject.transform.rotation = Quaternion.Slerp(parentObject.transform.rotation, targetRotation, elapsedTime / duration);
            yield return null;
        }
        
        if(parentObject != null)
           parentObject.transform.rotation = targetRotation;
    }
}
