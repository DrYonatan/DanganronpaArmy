using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HangmanLetter : MonoBehaviour
{
    public char letter = 'ש';
    public TextMeshProUGUI text;
    public Image image;
    public CanvasGroup canvasGroup;
    public int health = 3;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        text.text = letter.ToString();
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 0.5f);
        health = Random.Range(1, 4);
        UpdateColor();
    }

    // Update is called once per frame
    void Update()
    {
        CursorManager.instance.ReticleAsCursor();
        transform.localScale += Time.deltaTime * new Vector3(0.2f, 0.2f, 0.2f);
        image.transform.Rotate(0, 0, 360f * Time.deltaTime);
    }

    void OnMouseDown()
    {
        if (health > 0)
        {
            ReduceHealth();
        }
    }

    void OnMouseEnter()
    {
        
    }

    void ReduceHealth()
    {
        health--;
        UpdateColor();
    }

    void UpdateColor()
    {
        if(health == 1)
            image.color = Color.red;
        else if (health == 2)
        {
            image.color = new Color(1, 140f/255f, 140f/255f, 1f);
        }
        else
        {
            image.color = Color.white;
        }
    }
}
