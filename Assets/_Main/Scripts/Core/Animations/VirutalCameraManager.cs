using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirutalCameraManager : MonoBehaviour
{
    public static VirutalCameraManager instance { get; private set; }

    public Cinemachine.CinemachineVirtualCamera virtualCamera;
    public DollyCameraPitchControl pitchControl;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public void AssignVirtualCamera()
    {
        virtualCamera = GameObject.Find("World/Virtual Camera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        pitchControl = virtualCamera.GetComponent<DollyCameraPitchControl>();
    }

    public void DisableVirtualCamera()
    {
        if(virtualCamera)
        {
           virtualCamera.gameObject.SetActive(false);
        }
       

    }

    public void EnableVirtualCamera()
    {
        if(virtualCamera)
        {
            virtualCamera.gameObject.SetActive(true);
            MakeMainCameraFollowVirtualCamera();
        }
        
    
    }

    public void MakeMainCameraFollowVirtualCamera()
    {
        Camera.main.transform.position = virtualCamera.State.FinalPosition;
    }

    public IEnumerator SlideAcrossRoom(float duration)
    {
        float elapsedTime = 0f;

        float initialPosition = VirutalCameraManager.instance.virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition;
        float position = 0f;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            position += Time.deltaTime;
            VirutalCameraManager.instance.virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = position;
            yield return null;
        }
        
        elapsedTime = 0f;

        while(elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            position = Mathf.MoveTowards(position, initialPosition, Time.deltaTime * 4f);
            VirutalCameraManager.instance.virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = position;
            yield return null;
        }
    }
}
