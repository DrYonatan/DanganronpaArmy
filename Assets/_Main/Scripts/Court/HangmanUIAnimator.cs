using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HangmanUIAnimator : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public CanvasGroup canvasGroup;
    public Image lettersLeftCount;
    public Image lettersLeftGlow;
    
    public RawImage mist;

    public RawImage shade;

    public RawImage stars;
    public RawImage stars2;
    
    public RawImage circlePrefab;
    
    public float growFactor = 1.2f;
    public float growDuration = 0.3f;

    public float circlesGrowDuration = 1f;
    
    public Image silhouette;
    public RectTransform circles;

    public RectTransform blocksContainer;
    public HangmanLetterBlock blockPrefab;
    public List<HangmanLetterBlock> blockObjects = new List<HangmanLetterBlock>();
    public RectTransform letterObjectsContainer;
    public ScreenShatterManager screenShatterManager;
    public HangmanNowIUnderstand nowIUnderstand;
    
    public Color shadeFlashColor;

    private int lettersLeft;
    
    public void ShowHangmanUI()
    {
        Color c = mist.color;
        c.a = 0f;
        mist.color = c;
        
        mist.DOFade(1f, 3f)
            .SetLoops(-1, LoopType.Yoyo) // -1 = infinite
            .SetEase(Ease.Linear);

        
        Sequence seq = DOTween.Sequence();
        seq.Append(shade.DOColor(shadeFlashColor, 0.5f).SetLoops(2, LoopType.Yoyo));
        seq.AppendInterval(3f);
        seq.SetLoops(-1);
        
        silhouette.GetComponent<RectTransform>().DOShakeAnchorPos(5f, strength: new Vector2(5f, 5f), vibrato: 1, randomness: 90, snapping: false, fadeOut: false)
            .SetLoops(-1, LoopType.Restart);

        StartCoroutine(StarsGrow());
        SpawnCircles();
    }
    

    public void SetSilhouette(CharacterStand stand)
    {
        silhouette.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (1.8f - stand.heightPivot.localPosition.y - 0.05f) * 394.41f - 625f);
        silhouette.sprite = stand.spriteRenderer.sprite;
    }

    public void SetLettersLeftCount(int lettersLeft)
    {
        float initialY = lettersLeftCount.GetComponent<RectTransform>().anchoredPosition.y;
        Image newLettersLeftCount = Instantiate(lettersLeftCount, lettersLeftCount.transform.parent);
        newLettersLeftCount.transform.SetAsFirstSibling();
        newLettersLeftCount.transform.position = lettersLeftCount.transform.position;
        newLettersLeftCount.sprite = Resources.Load<Sprite>($"Images/UI/Hangman/Numbers/{lettersLeft}");
        lettersLeftCount.GetComponent<RectTransform>().DOAnchorPosY(initialY - 10f, 0.2f);
        lettersLeftCount.DOFade(0f, 0.2f);

        newLettersLeftCount.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 10f);
        newLettersLeftCount.GetComponent<RectTransform>().DOAnchorPosY(initialY, 0.2f);
        newLettersLeftCount.DOFade(1f, 0.2f);
        
        Destroy(lettersLeftCount.gameObject, 2f);
        lettersLeftCount = newLettersLeftCount;
    }

    public IEnumerator FinishAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        lettersLeftGlow.DOFade(1f, 0.2f).SetLoops(2, LoopType.Yoyo);
        yield return new WaitForSeconds(0.5f);
        yield return FlashBlocks();
        nowIUnderstand.gameObject.SetActive(true);
        yield return nowIUnderstand.Show();
        nowIUnderstand.gameObject.SetActive(false);
        screenShatterManager = Instantiate(screenShatterManager);
        yield return screenShatterManager.ScreenShatter();
    }

    IEnumerator FlashBlocks()
    {
        for(int i = 0; i < blockObjects.Count; i++)
        {
            blockObjects[i].glow.DOFade(1f, 0.2f).SetLoops(2, LoopType.Yoyo).SetDelay(i * 0.02f);
        }
        yield return new WaitForSeconds(blockObjects.Count * 0.2f);
    }
    
    public void HideLetterObjects()
    {
        letterObjectsContainer.GetComponent<CanvasGroup>().DOFade(0f, 0.5f);
    }
    
    public IEnumerator GenerateLetterBlocks(List<Letter> letters)
    {
        lettersLeft = letters.Count;
        SetLettersLeftCount(lettersLeft);
        
        foreach (Letter letter in letters)
        {
            HangmanLetterBlock block = Instantiate(blockPrefab, blocksContainer);
            block.letterRepresented = letter.letter;
            blockObjects.Add(block);
        }
        BlocksStartAnimation();
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(AquireBlocks(letters));
    }

    public void DecreaseLettersLeftCount()
    {
        lettersLeft--;
        SetLettersLeftCount(lettersLeft);
    }

    public IEnumerator AquireBlocks(List<Letter> letters)
    {
        foreach (Letter letter in letters)
        {
            HangmanLetterBlock block = blockObjects.Find((block) => block.letterRepresented == letter.letter);
            if (letter.isAquired)
            {
                DecreaseLettersLeftCount();
                block.GetAquired();
                yield return new WaitForSeconds(0.6f);
            }
        }
        yield return null;
    }

    IEnumerator StarsGrow()
    {
        Sequence seq1 = DOTween.Sequence();
        // Scale and fade happen together
        seq1.Append(stars.transform.DOScale(growFactor, growDuration).SetEase(Ease.OutQuad));
        seq1.Join(stars.DOFade(0f, growDuration));
        // Reset instantly
        seq1.Append(stars.transform.DOScale(1f, 0f));
        seq1.Append(stars.DOFade(1f, 0f));
        seq1.SetLoops(-1);

        yield return new WaitForSeconds(growDuration / 2);

        Sequence seq2 = DOTween.Sequence();
        // Same logic for stars2, with smaller growth
        seq2.Append(stars2.transform.DOScale(growFactor * 0.8f, growDuration).SetEase(Ease.OutQuad));
        seq2.Join(stars2.DOFade(0f, growDuration));
        seq2.Append(stars2.transform.DOScale(1f, 0f));
        seq2.Append(stars2.DOFade(1f, 0f));
        seq2.SetLoops(-1);
    }

    void SpawnCircles()
    {
        Sequence pattern = DOTween.Sequence().SetLoops(-1); // infinite loop

        // 1st circle
        pattern.AppendCallback(() => SpawnCircle());
        pattern.AppendInterval(0.5f);

        // 2nd circle
        pattern.AppendCallback(() => SpawnCircle());
        pattern.AppendInterval(0.2f);

        // 3rd circle
        pattern.AppendCallback(() => SpawnCircle());
        pattern.AppendInterval(0.5f);
    }
    
    void SpawnCircle()
    {
        RawImage circle = Instantiate(circlePrefab, circles);
        circle.color = Color.blue;
        circle.transform.localScale = Vector3.one;

        // Sequence for this individual circle
        Sequence circleSeq = DOTween.Sequence();

        circleSeq.Append(circle.transform.DOScale(5f, circlesGrowDuration).SetEase(Ease.OutQuad));
        circleSeq.Join(circle.DOColor(Color.red, circlesGrowDuration));
        circleSeq.Join(circle.transform.DOLocalRotate(new Vector3(0, 0, 60f), circlesGrowDuration));

        // Destroy the circle after animation finishes
        circleSeq.OnComplete(() => Destroy(circle.gameObject));
    }
    
    void Update()
    {
        shade.transform.Rotate(0, 0 , -40f * Time.deltaTime);
    }

    public void AquireBlock(int index)
    {
        DecreaseLettersLeftCount();
        blockObjects[index].GetAquired();
    }

    public void TurnBlockIntoCurrent(int index)
    {
        blockObjects[index].TurnIntoCurrentLetter();
    }

    public void BlocksStartAnimation()
    {
        foreach (HangmanLetterBlock block in blockObjects)
        {
            block.canvasGroup.alpha = 0f;
            block.transform.DOScaleY(0f, 0f);
        }
        Sequence seq = DOTween.Sequence();

        float delayBetweenBlocks = 0.1f; // time between each block starting
        float animDuration = 0.5f;       // duration for each block's grow/fade animation

        for (int i = 0; i < blockObjects.Count; i++)
        {
            HangmanLetterBlock block = blockObjects[i];

            // Create a tween for this block
            Tween scaleTween = block.transform.DOScaleY(1f, animDuration).SetEase(Ease.OutBack);
            Tween alphaTween = block.canvasGroup.DOFade(1f, animDuration);

            // Add both tweens to the sequence so they run together
            seq.Insert(i * delayBetweenBlocks, scaleTween);
            seq.Insert(i * delayBetweenBlocks, alphaTween);
        }
        
        BlocksBlinkRandomly();
    }
    
    
    public void BlocksBlinkRandomly()
    {
        int blinkCount = 5;              // total number of blink events
        float blinkDuration = 0.1f;      // fade out/in time
        float timeBetweenBlinks = 0.1f;  // pause between blink events

        Sequence blinkSequence = DOTween.Sequence();

        for (int i = 0; i < blinkCount; i++)
        {
            // Pick a random number of blocks to blink this round
            int blocksToBlink = Random.Range(1, blockObjects.Count + 1);

            // Pick unique random indices
            var indices = new System.Collections.Generic.HashSet<int>();
            while (indices.Count < blocksToBlink)
            {
                indices.Add(Random.Range(0, blockObjects.Count));
            }

            // For each selected block, create a quick fade out and fade in
            foreach (int index in indices)
            {
                HangmanLetterBlock block = blockObjects[index];
                // Fade out then back in
                blinkSequence.Insert(i * (blinkDuration + timeBetweenBlinks), 
                    block.canvasGroup.DOFade(0f, blinkDuration / 2).SetLoops(2, LoopType.Yoyo));
            }
        }
    }

    public void UpdateTimerText(TimeSpan time)
    {
        timerText.text = time.ToString(@"mm\:ss\:ffff");
    }
    
    
}
