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
        if (instance == null)
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
        if (virtualCamera)
        {
            virtualCamera.gameObject.SetActive(false);
        }
    }

    public void EnableVirtualCamera()
    {
        if (virtualCamera)
        {
            virtualCamera.gameObject.SetActive(true);
            MakeMainCameraFollowVirtualCamera();
        }
    }

    public void MakeMainCameraFollowVirtualCamera()
    {
        Camera.main.transform.position = virtualCamera.State.FinalPosition;
    }

    public IEnumerator SlideAcrossRoom(float duration, Vector3 slidingTrackPoisition)
    {
        float elapsedTime = 0f;

        CinemachineTrackedDolly dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        Vector3 initialTrackPosition = dolly.m_Path.transform.position;
        float initialPosition = dolly.m_PathPosition;
        dolly.m_Path.transform.position = slidingTrackPoisition;
        float position = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            position += Time.deltaTime;
            dolly.m_PathPosition = position;
            yield return null;
        }

        elapsedTime = 0f;
        
        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            dolly.m_PathPosition = Mathf.Lerp(position, initialPosition,  elapsedTime / 0.5f);;
            dolly.m_Path.transform.position = Vector3.Lerp(slidingTrackPoisition, initialTrackPosition, elapsedTime / 0.5f);
            yield return null;
        }
        
        dolly.m_PathPosition = initialPosition;
        dolly.m_Path.transform.position = initialTrackPosition;
    }
}