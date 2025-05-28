using UnityEngine;
using Cinemachine;

[ExecuteAlways]
public class CM_YawOffset : CinemachineExtension
{
    [Tooltip("Degrees to rotate around the path's up-axis")]
    public float yawOffset = 90f;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        // After the Body (i.e. Dolly) has set pos+rot, add your yaw
        if (stage == CinemachineCore.Stage.Body)
        {
            // rotate around the 'up' axis of the camera
            state.RawOrientation = state.RawOrientation
                * Quaternion.Euler(0, yawOffset, 0);
        }
    }
}
