using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CursorManager : MonoBehaviour
{
    public int speed = 30;
    public RectTransform cursor;
    public Canvas canvas;
    public Transform reticle;
    public GameObject magnifyingGlass;
    public GameObject conversationIcon;
    public TextMeshProUGUI interactableNameText;
    public CanvasGroup interactableNameCanvasGroup;
    private Coroutine fadeCoroutine;
    private Vector3 originalScale;
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1.1f);

    public static CursorManager instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        originalScale = cursor.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        int actualSpeed = speed;
        Vector3 goalScale = originalScale;
        if (WorldManager.instance != null)
            if (WorldManager.instance.currentRoom.currentInteractable != null)
            {
                actualSpeed *= 4;
                WorldManager.instance.currentRoom.currentInteractable.OnLook();
            }
            else
            {
                ShowOrHideMagnifyingGlass(false);
                ShowOrHideConversationIcon(false);
            }
        else if(GameLoop.instance != null)
        {
            if(GameLoop.instance.currentAimedText != null)
            {
                actualSpeed = 0;
                goalScale = hoverScale;  
            }
        }             
        cursor.localScale = Vector3.Lerp(cursor.localScale, goalScale, Time.deltaTime * 20f);
        reticle.Rotate(0, 0, actualSpeed * Time.deltaTime);
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
        cursor.gameObject.SetActive(false);
    }
    public void Show()
    {
        cursor.gameObject.SetActive(true);
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
        if(fadeCoroutine != null)
        StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(ShowingOrHidingInteractableName(0.1f, show ? 1f : 0f));
    }

    IEnumerator ShowingOrHidingInteractableName(float duration, float target)
    {
        float elapsedTime = 0f;

        float start = interactableNameCanvasGroup.alpha;
        while(elapsedTime < duration)
        {
            interactableNameCanvasGroup.alpha = Mathf.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        interactableNameCanvasGroup.alpha = target;
    }
}