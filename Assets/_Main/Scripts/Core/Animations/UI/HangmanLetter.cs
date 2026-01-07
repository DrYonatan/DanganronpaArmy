using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HangmanLetter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    public char letter = 'A';
    public TextMeshProUGUI text;
    public Image image;
    public CanvasGroup canvasGroup;
    public int maxHealth = 3;
    public int health = 3;
    public Color maxLifeColor = Color.yellow;
    public Color minLifeColor = Color.red;
    public Image clickEffect;
    public Image explosionEffect;
    public AudioClip clickSound;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        TrialCursorManager.instance.isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TrialCursorManager.instance.isHovering = false;
    }

    void ExplosionEffect()
    {
        Image explosion = Instantiate(explosionEffect, transform.parent);
        explosion.transform.position = transform.position;
        explosion.transform.DOScale(5f, 0.2f);
        explosion.DOFade(0f, 0.2f);
        
        Image effect = Instantiate(clickEffect, transform.parent);
        effect.transform.position = transform.position;
        effect.transform.DOScale(5f, 0.5f);
        effect.DOFade(0f, 0.5f);
        
        Destroy(explosion.gameObject, 1f);
        Destroy(effect.gameObject, 1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ExplosionEffect();
        SoundManager.instance.PlaySoundEffect(clickSound);
        if (health > 1)
        {
            ReduceHealth();
        }
        else
        {
            rectTransform.DOKill();
            canvasGroup.DOKill();
            HangmanManager.instance.CheckLetter(letter);
            Kill();
        }
    }

    void ReduceHealth()
    {
        health--;
        UpdateColor();
        Shake();
    }

    public void Kill()
    {
        TrialCursorManager.instance.isHovering = false;
        Destroy(gameObject);
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
