using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffectController : MonoBehaviour
{
    public Vector3 position;
    public Vector3 rotation;
    public float zoom;
    List<Coroutine> operations = new List<Coroutine>();
    

    // public void Process()
    // {
    //     if(effect != null)
    //     {
    //         if(effect.timeLimit > timer)
    //         {
    //             effect.Apply(this);
    //         }
            
    //     }
    //     timer += Time.deltaTime;
    // }

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
        position = Vector3.zero;
        rotation = Vector3.zero;
        zoom = 0f;
        foreach(Coroutine operation in operations)
        {
            StopCoroutine(operation);
        }
    }
}
