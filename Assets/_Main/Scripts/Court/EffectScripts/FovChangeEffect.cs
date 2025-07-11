using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Behaviour Editor/Camera Effect/Change Fov"))]
public class FovChangeEffect : CameraEffect
{
   public float targetFov;
   public override IEnumerator Apply(CameraEffectController effectController)
   {
      float elapsedTime = 0f;
      while (elapsedTime < timeLimit)
      {
         effectController.camera.fieldOfView = Mathf.Lerp(effectController.camera.fieldOfView, targetFov, Time.deltaTime / timeLimit);
         elapsedTime += Time.deltaTime;
         yield return null;
      }
   }
}
