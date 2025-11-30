using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenMainMenu : MonoBehaviour
{
    public static TitleScreenMainMenu instance { get; private set; }
    
    public List<TitleScreenSubMenu> subMenus;
    public TitleScreenSubMenu activeSubMenu;
    private Stack<TitleScreenSubMenu> subMenuStack = new ();

    public Image donkey;
    public Image konga;
    public Image underlay;
    public Image leftCharacter;
    public Image rightCharacter;

    void Awake()
    {
        instance = this;
        subMenuStack.Push(activeSubMenu);
        Cursor.lockState =  CursorLockMode.Locked;
        StartAnimation();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& subMenuStack.Count > 1)
        {
            ReturnToPrevMenu();
        }
    }

    public void SwitchMenus(TitleScreenSubMenu menu)
    {
        subMenuStack.Push(menu);
        StartCoroutine(SwitchMenusAnimation(activeSubMenu, menu));
        activeSubMenu = menu;
    }

    private void ReturnToPrevMenu()
    {
        StartCoroutine(SwitchMenusAnimation(subMenuStack.Pop(), subMenuStack.Peek()));
        activeSubMenu = subMenuStack.Peek();
    }

    private IEnumerator SwitchMenusAnimation(TitleScreenSubMenu prev, TitleScreenSubMenu next)
    {
        prev.OutroAnimation();
        
        yield return new WaitForSeconds(0.2f);
        
        prev.gameObject.SetActive(false);
        next.gameObject.SetActive(true);
        
        next.Initialize();
    }

    private void StartAnimation()
    {
        activeSubMenu.gameObject.SetActive(false);
        
        donkey.rectTransform.localScale = Vector3.zero;
        konga.rectTransform.localScale = Vector3.zero;
        underlay.DOFade(0f, 0f);
        underlay.rectTransform.localScale = Vector3.one * 0.5f;
        float originalLeft = leftCharacter.rectTransform.anchoredPosition.x;
        float originalRight = rightCharacter.rectTransform.anchoredPosition.x;
        
        leftCharacter.rectTransform.DOAnchorPosX(originalLeft - 800f, 0);
        rightCharacter.rectTransform.DOAnchorPosX(originalRight + 800f, 0);
        
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.2f);
        seq.Append(donkey.rectTransform.DOScale(1f, 0.2f));
        seq.AppendInterval(0.2f);
        seq.Append(konga.rectTransform.DOScale(1f, 0.2f));
        seq.Append(underlay.DOFade(1f, 0.2f));
        seq.Join(underlay.rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
        seq.Join(leftCharacter.rectTransform.DOAnchorPosX(originalLeft, 0.2f));
        seq.Join(rightCharacter.rectTransform.DOAnchorPosX(originalRight, 0.2f));

        seq.OnComplete(() => InitializeFirstMenu());



    }

    private void InitializeFirstMenu()
    {
        activeSubMenu.gameObject.SetActive(true);
        activeSubMenu.Initialize();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}