using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HangmanLetter : MonoBehaviour
{
    private RectTransform rectTransform;
    public char letter = 'ש';
    public TextMeshProUGUI text;
    public Image image;
    public CanvasGroup canvasGroup;
    public int maxHealth = 3;
    public int health = 3;
    public Color maxLifeColor = Color.white;
    public Color minLifeColor = Color.red;
    void Start()
    {
        rectTransform =  GetComponent<RectTransform>();
        Cursor.lockState = CursorLockMode.Confined;
        text.text = letter.ToString();
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 0.5f);
        health = Random.Range(1, maxHealth + 1);
        UpdateColor();
    }

    // Update is called once per frame
    void Update()
    {
        CursorManager.instance.ReticleAsCursor();
        transform.localScale += Time.deltaTime * new Vector3(0.2f, 0.2f, 0.2f);
        image.transform.Rotate(0, 0, 360f * Time.deltaTime);
    }

    void OnMouseEnter()
    {
        TrialCursorManager.instance.isHovering = true;
    }

    void OnMouseExit()
    {
        TrialCursorManager.instance.isHovering = false;
    }

    void OnMouseDown()
    {
        if (health > 0)
        {
            ReduceHealth();
        }
    }

    void ReduceHealth()
    {
        health--;
        UpdateColor();
        Shake();
    }

    void Shake()
    {
        rectTransform.DOKill();
        rectTransform.DOShakeAnchorPos(0.2f, strength: new Vector2(10f, 10f), vibrato: 10, randomness: 90, snapping: true,
            fadeOut: false);
    }

    void UpdateColor()
    {
        if (health == maxHealth)
        {
            image.color = maxLifeColor;
        }
        else if (health == 1)
        {
            image.color = minLifeColor;
        }
        else
        {
            float t = 1f - (health - 1f) / (maxHealth - 1f);
            image.color = Color.Lerp(maxLifeColor, minLifeColor, t);
        }
    }
}
