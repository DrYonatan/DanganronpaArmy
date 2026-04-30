using System.Collections;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class CursorManager : MonoBehaviour
{
    public int speed = 30;
    public RectTransform cursor;
    private CanvasGroup canvasGroup;
    public Canvas canvas;
    public Transform reticle;
    public GameObject magnifyingGlass;
    public GameObject conversationIcon;
    public TextMeshProUGUI interactableNameText;
    public CanvasGroup interactableNameCanvasGroup;
    public AudioClip hoverSound;
    private Coroutine fadeCoroutine;
    public bool isHovering = false;


    public static CursorManager instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        canvasGroup = cursor.GetComponent<CanvasGroup>();
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        ManageHover();
    }

    protected virtual void ManageHover()
    {
        int actualSpeed = speed;
        if (WorldManager.instance != null)
        {
            Interactable currentInteractable = WorldManager.instance.currentRoom?.currentInteractable;
            if (currentInteractable != null)
            {
                actualSpeed *= 4;
                if (!currentInteractable.isAlreadyLooking)
                {
                    SoundManager.instance.PlaySoundEffect(hoverSound);
                }
                currentInteractable.OnLook();
            }
            else
            {
                ShowOrHideMagnifyingGlass(false);
                ShowOrHideConversationIcon(false);
            }
        }

        reticle.Rotate(0, 0, actualSpeed * Time.unscaledDeltaTime);
    }

    public void ReticleAsCursor()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out pos
        );
        cursor.anchoredPosition = pos;
    }

    public void Hide()
    {
        if(cursor.gameObject.activeSelf)
           canvasGroup.DOFade(0f, 0.1f).OnComplete(() => cursor.gameObject.SetActive(false));
    }

    public void Show()
    {
        cursor.gameObject.SetActive(true);
        canvasGroup.DOFade(1f, 0.1f);
    }

    public void ShowOrHideMagnifyingGlass(bool show)
    {
        magnifyingGlass.SetActive(show);
    }

    public void ShowOrHideConversationIcon(bool show)
    {
        conversationIcon.SetActive(show);
    }

    public void ShowOrHideInteractableName(bool show, string name)
    {
        interactableNameText.text = name;
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(ShowingOrHidingInteractableName(0.1f, show ? 1f : 0f));
    }

    IEnumerator ShowingOrHidingInteractableName(float duration, float target)
    {
        float elapsedTime = 0f;

        float start = interactableNameCanvasGroup.alpha;
        while (elapsedTime < duration)
        {
            interactableNameCanvasGroup.alpha = Mathf.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        interactableNameCanvasGroup.alpha = target;
    }
}