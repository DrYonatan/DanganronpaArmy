using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviour Editor/Camera Effect/Shake")]
public class ShakeCameraEffect : CameraEffect
{
    [SerializeField] Vector3 limits;
    [SerializeField] int intensity = 10;


    public override IEnumerator Apply(CameraEffectController effectController)
    {
        float elapsedTime = 0f;
        while(elapsedTime < timeLimit)
        {
           if (Time.frameCount % intensity == 0)
           {
            Vector3 newPosition = new Vector3(
               Random.Range(-limits.x/100, limits.x/100),
               Random.Range(-limits.y/100, limits.y/100),
               Random.Range(-limits.z/100, limits.z/100)
               );
            effectController.position = newPosition;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        
    }
}
