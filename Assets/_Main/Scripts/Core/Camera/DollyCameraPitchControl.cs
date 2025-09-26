using UnityEngine;
using Cinemachine;

[ExecuteAlways]
[DisallowMultipleComponent]
public class DollyCameraPitchControl : CinemachineExtension
{
    [Range(-90f, 90f)]
    public float pitch = 0f;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Aim)
        {
            // Calculate pitch relative to the current camera orientation
            Vector3 right = state.RawOrientation * Vector3.right;
            Quaternion pitchRot = Quaternion.AngleAxis(pitch, right);

            state.RawOrientation = pitchRot * state.RawOrientation;
        }
    }
}
