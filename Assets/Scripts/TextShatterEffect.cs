using System.Collections;
using TMPro;
using UnityEngine;

public class TextShatterEffect : MonoBehaviour
{
    public float explosionForce = 5f;
    public float explosionRadius = 3f;
    public float letterLifetime = 2f;
    public float spinAmount = 10f;

    public void Shatter(GameObject textToShatter)
    {
        TextMeshPro textMesh = textToShatter.GetComponent<TextMeshPro>();
        textMesh.ForceMeshUpdate();
        TMP_TextInfo textInfo = textMesh.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            Vector3[] sourceVerts = textInfo.meshInfo[materialIndex].vertices;

            // Get world space positions of character's quad
            Vector3[] worldVerts = new Vector3[4];
            for (int j = 0; j < 4; j++)
            {
                worldVerts[j] = textMesh.transform.TransformPoint(sourceVerts[vertexIndex + j]);
            }

            // Calculate center of the character in world space
            Vector3 worldCenter = (worldVerts[0] + worldVerts[2]) / 2f;

            // Create a new GameObject for the letter
            GameObject letterObj = new GameObject("Letter_" + i);
            letterObj.transform.position = worldCenter;

            // Adjust vertices to be local to the new object
            for (int j = 0; j < 4; j++)
            {
                worldVerts[j] -= worldCenter;
            }

            // Create mesh
            Mesh mesh = new Mesh();
            mesh.vertices = new Vector3[] { worldVerts[0], worldVerts[1], worldVerts[2], worldVerts[3] };
            mesh.uv = new Vector2[]
            {
                textInfo.meshInfo[materialIndex].uvs0[vertexIndex + 0],
                textInfo.meshInfo[materialIndex].uvs0[vertexIndex + 1],
                textInfo.meshInfo[materialIndex].uvs0[vertexIndex + 2],
                textInfo.meshInfo[materialIndex].uvs0[vertexIndex + 3]
            };
            mesh.triangles = new int[] { 0, 1, 2, 2, 3, 0 };
            mesh.RecalculateNormals();

            // Add components
            MeshFilter mf = letterObj.AddComponent<MeshFilter>();
            mf.mesh = mesh;

            MeshRenderer mr = letterObj.AddComponent<MeshRenderer>();
            mr.material = textMesh.fontMaterial;

            Rigidbody rb = letterObj.AddComponent<Rigidbody>();
            // Get the direction from the text to the camera
            Vector3 toCamera = (Camera.main.transform.position - letterObj.transform.position).normalized;

            // Create a random horizontal direction
            Vector3 randomHorizontal = Vector3.right * Random.Range(-1f, 1f) + Vector3.forward * Random.Range(-1f, 1f);
            randomHorizontal.Normalize();

            Vector3 randomVertical = Vector3.up * Random.Range(-1f, 1f) + Vector3.forward * Random.Range(-1f, 1f);

            // Blend the horizontal randomness with the direction to the camera
            Vector3 dir = (toCamera + randomHorizontal * 0.5f + randomVertical * 0.5f).normalized;

            rb.AddForce(dir * explosionForce, ForceMode.Impulse);
            Vector3 camForward = Camera.main.transform.forward;
            rb.AddTorque(camForward * Random.Range(-spinAmount, spinAmount), ForceMode.Impulse);

            Destroy(letterObj, letterLifetime);
        }

        // Optionally hide the original text
        textMesh.gameObject.SetActive(false);
    }
}
