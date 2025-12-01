using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Image glow;
    public Image cylinder;
    public Image glowingCylinder;
    public Image bigRing;
    public Image smallRing;
    public Image blackOverlay;

    public List<Sprite> availableFaceSprites;

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

    private Sprite GetRandomSprite()
    {
        int index = Random.Range(0, availableFaceSprites.Count);
        return availableFaceSprites[index];
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
        glow.DOFade(0f, 0f);

        rightCharacter.sprite = GetRandomSprite();
        
        leftCharacter.rectTransform.DOAnchorPosX(originalLeft - 800f, 0);
        rightCharacter.rectTransform.DOAnchorPosX(originalRight + 800f, 0);
        
        cylinder.DOFade(0f, 0f);
        glowingCylinder.DOFade(0f, 0f);
        bigRing.DOFade(0f, 0f);
        smallRing.DOFade(0f, 0f);
        
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.2f);
        seq.Append(donkey.rectTransform.DOScale(1f, 0.2f));
        seq.AppendInterval(0.2f);
        seq.Append(konga.rectTransform.DOScale(1f, 0.2f));
        seq.Append(underlay.DOFade(1f, 0.2f));
        seq.Join(underlay.rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
        seq.Join(leftCharacter.rectTransform.DOAnchorPosX(originalLeft, 0.2f));
        seq.Join(rightCharacter.rectTransform.DOAnchorPosX(originalRight, 0.2f));
        seq.Append(glow.DOFade(1f, 0.2f));
        seq.Append(glow.DOFade(0f, 0.2f));
        seq.Join(glow.rectTransform.DOScale(1.2f, 0.2f));

        seq.OnComplete(() => InitializeFirstMenu());
    }

    private void InitializeFirstMenu()
    {
        activeSubMenu.gameObject.SetActive(true);
        activeSubMenu.Initialize();
        
        cylinder.DOFade(1f, 0.1f);
        bigRing.DOFade(1f, 0.1f);
        smallRing.DOFade(1f, 0.1f);
        
        cylinder.rectTransform.DORotate(new Vector3(0f, 0f, 360f), 4f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
        bigRing.rectTransform.DORotate(new Vector3(0f, 0f, 360f), 7f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
        smallRing.rectTransform.DORotate(new Vector3(0f, 0f, 360f), 3f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);

        underlay.DOFade(0.5f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    public void GoToGameAnimation(string sceneToLoad)
    {
        cylinder.rectTransform.DOKill();
        Sequence seq = DOTween.Sequence();
        seq.Join(bigRing.rectTransform.DOScale(6f, 0.5f));
        seq.Join(smallRing.rectTransform.DOScale(6f, 0.5f));
        seq.Join(glowingCylinder.DOFade(1f, 0.1f));
        seq.Append(blackOverlay.DOFade(1f, 0.2f));
        
        seq.OnComplete(() => GoToGame(sceneToLoad));
    }

    private void GoToGame(string sceneToLoad)
    {
        DOTween.KillAll();
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}