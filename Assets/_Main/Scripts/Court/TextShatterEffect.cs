using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TextShatterEffect : MonoBehaviour
{
    public float explosionForce = 5f;
    public float letterLifetime = 2f;
    public float spinAmount = 10f;

    public GameObject hitEffectPrefab;

    public void Shatter(GameLoop.TextLine textLine)
    {
        SoundManager.instance.PlaySoundEffect("shatter");

        TextMeshPro textToSeperate = textLine.textMeshPro;
        string text = Regex.Replace(textToSeperate.text, "<.*?>", "");
        Vector3 startPosition = textToSeperate.transform.position;
        float charSpacing = 0.15f;
        bool isStatementCharacter = false;

        for (int i = 0; i < text.Length; i++)
        {
            GameObject charObj = new GameObject($"Char_{i}");
            charObj.transform.SetParent(textToSeperate.transform.parent);

            TextMeshPro tmp = charObj.AddComponent<TextMeshPro>();
            tmp.text = text[i].ToString();
            tmp.font = textToSeperate.font;
            tmp.fontSize = textToSeperate.fontSize;
            if(i >= textLine.correctCharacterIndexBegin && i <= textLine.correctCharacterIndexEnd)
            isStatementCharacter = true;
            
            if(isStatementCharacter)
            tmp.color = new Color32(255, 165, 0, 255); // give it an orange color
            tmp.alignment = textToSeperate.alignment;

            charObj.transform.position = startPosition +
                                         textToSeperate.transform.right * charSpacing * (text.Length / 2f - i);
            
            Rigidbody rb = charObj.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.drag = 0.5f;
            // Get the direction from the text to the camera
            Vector3 toCamera = (GameLoop.instance.statementsCamera.transform.position - textToSeperate.transform.position).normalized;
            
            // Blend the horizontal randomness with the direction to the camera
            Vector3 dir = (toCamera).normalized;
            
            float forceFactor = Random.Range(0.7f, 1.8f);
            float spinFactor = 2f;

            if(!isStatementCharacter)
            {
                forceFactor = 0.1f;
                spinFactor = 0.1f;
            }

            rb.AddForce(dir * explosionForce * forceFactor, ForceMode.Impulse);
            Vector3 camForward = GameLoop.instance.statementsCamera.transform.forward;
            rb.AddTorque(camForward * Random.Range(-spinAmount, spinAmount) * spinFactor, ForceMode.Impulse);

            StartCoroutine(FadeCharacterAway(charObj.GetComponent<TextMeshPro>(), 1f));
            Destroy(charObj, letterLifetime);
            
        }
        
        textToSeperate.gameObject.SetActive(false);

    }

    public IEnumerator Deflect(TextMeshPro textToDeflect, Vector3 hitPosition)
    {
        string text = textToDeflect.text;
        for (int i = text.Length-1; i >= 0; i--)
        {
            textToDeflect.text = text;
            textToDeflect.ForceMeshUpdate();            
            GameObject charObj = new GameObject($"Char_{i}");
            charObj.transform.SetParent(textToDeflect.transform.parent);

            TextMeshPro tmp = charObj.AddComponent<TextMeshPro>();
            tmp.text = text[i].ToString();
            tmp.font = textToDeflect.font;
            tmp.fontSize = textToDeflect.fontSize;
            tmp.alignment = textToDeflect.alignment;

            charObj.transform.position = hitPosition;
            charObj.transform.rotation = textToDeflect.transform.rotation * Quaternion.Euler(0, -90f, 0);

            if(text.Length > 1)
            text = text.Remove(i, 1);

            Rigidbody rb = charObj.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.drag = 0.5f;
            // Get the direction from the text to the camera            
            // Blend the horizontal randomness with the direction to the camera
            Vector3 randomVertical = Vector3.up * Random.Range(0.4f, -0.4f);
            Vector3 dir = charObj.transform.right * -1 + randomVertical;
            
            
            rb.AddForce(dir * explosionForce, ForceMode.Impulse);

            Destroy(charObj, letterLifetime);

            textToDeflect.transform.position += textToDeflect.transform.right * 0.6f;


            yield return new WaitForSeconds(0.05f);
            
        }
        
        textToDeflect.gameObject.SetActive(false);
    }

    public void Explosion(Vector3 position)
    {
        GameObject effect = Instantiate(hitEffectPrefab, position, Quaternion.identity);
        effect.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Destroy(effect, 2f);
    }

    public IEnumerator FadeCharacterAway(TextMeshPro charObj, float duration)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            float alpha = Mathf.MoveTowards(1f, 0f, elapsedTime / duration);
            charObj.color = new Color(charObj.color.r, charObj.color.g, charObj.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}