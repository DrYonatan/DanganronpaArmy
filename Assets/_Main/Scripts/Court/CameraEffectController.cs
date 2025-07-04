using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffectController : MonoBehaviour
{
    public Transform cameraTransform;
    List<Coroutine> operations = new List<Coroutine>();

    public void StartEffect(CameraEffect effect)
    {
        if(effect != null)
        {
            var operation = StartCoroutine(effect.Apply(this));
            operations.Add(operation);
        }
        
    }

    public void Reset()
    {
        foreach(Coroutine operation in operations)
        {
            StopCoroutine(operation);
        }
    }
}
