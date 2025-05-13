using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TextShatterEffect : MonoBehaviour
{
    public float explosionForce = 5f;
    public float letterLifetime = 2f;
    public float spinAmount = 10f;

    public void Shatter(TextMeshPro textToSeperate)
    {
        string text = Regex.Replace(textToSeperate.text, "<.*?>", "");
        Vector3 startPosition = textToSeperate.transform.position;
        float charSpacing = 0.15f;

        for (int i = 0; i < text.Length; i++)
        {
            GameObject charObj = new GameObject($"Char_{i}");
            charObj.transform.SetParent(textToSeperate.transform.parent);

            TextMeshPro tmp = charObj.AddComponent<TextMeshPro>();
            tmp.text = text[i].ToString();
            tmp.font = textToSeperate.font;
            tmp.fontSize = textToSeperate.fontSize;
            tmp.color = textToSeperate.color;
            tmp.alignment = textToSeperate.alignment;

            charObj.transform.position = startPosition +
                                         textToSeperate.transform.right * charSpacing * (i - text.Length / 2f);
            
            Rigidbody rb = charObj.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.drag = 0.5f;
            // Get the direction from the text to the camera
            Vector3 toCamera = (Camera.main.transform.position - textToSeperate.transform.position).normalized;

            // Create a random horizontal direction
            Vector3 randomHorizontal = Vector3.right * Random.Range(-0.01f, 0.01f) + Vector3.forward * Random.Range(-1f, 1f);
            randomHorizontal.Normalize();
            
            // Blend the horizontal randomness with the direction to the camera
            Vector3 dir = (toCamera).normalized;
            
            float forceFactor = Random.Range(0.7f, 1.8f);

            rb.AddForce(dir * explosionForce * forceFactor, ForceMode.Impulse);
            Vector3 camForward = Camera.main.transform.forward;
            rb.AddTorque(camForward * Random.Range(-spinAmount, spinAmount), ForceMode.Impulse);

            Destroy(charObj, letterLifetime);
        }
        
        textToSeperate.gameObject.SetActive(false);

    }

    public void Explosion()
    {
        
    }
}