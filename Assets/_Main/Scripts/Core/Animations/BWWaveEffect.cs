using UnityEngine;

public class BWWaveEffect : MonoBehaviour
{
    public Material material;
    public float radius = 0f;
    public float speed = 0.3f;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        radius += speed * Time.deltaTime;
        material.SetFloat("_Radius", radius);
        Graphics.Blit(src, dst, material);
    }
}