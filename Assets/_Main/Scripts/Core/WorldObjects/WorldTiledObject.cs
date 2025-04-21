using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class WorldTiledObject : MonoBehaviour
{
    public Vector2 tilesPerUnit = new Vector2(1, 1); // How many tiles per world unit

    void Start()
    {
        ApplyTiling();
    }

    void ApplyTiling()
    {
        var rend = GetComponent<Renderer>();
        Vector3 scale = transform.lossyScale;

        // Assumes tiling on XZ surface (like a floor)
        Vector2 tiling = new Vector2(scale.x * tilesPerUnit.x, scale.z * tilesPerUnit.y);

        rend.material.mainTextureScale = tiling;
    }
}

