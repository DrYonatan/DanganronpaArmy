using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HangmanUIAnimator : MonoBehaviour
{
    public RawImage mist;

    public RawImage shade;

    public RawImage stars;
    public RawImage stars2;
    
    public RawImage circlePrefab;
    
    public float growFactor = 1.2f;
    public float growDuration = 0.3f;

    public float circlesGrowDuration = 1f;
    
    public RectTransform silhouette;
    public RectTransform circles;

    public RectTransform blocksContainer;
    public HangmanLetterBlock blockPrefab;
    public List<HangmanLetterBlock> blockObjects = new List<HangmanLetterBlock>();


    void Start()
    {
        Color c = mist.color;
        c.a = 0f;
        mist.color = c;
        
        mist.DOFade(1f, 3f)
            .SetLoops(-1, LoopType.Yoyo) // -1 = infinite
            .SetEase(Ease.Linear);  
        
        silhouette.DOShakeAnchorPos(5f, strength: new Vector2(5f, 5f), vibrato: 1, randomness: 90, snapping: false, fadeOut: false)
            .SetLoops(-1, LoopType.Restart);

        StartCoroutine(StarsGrow());
        SpawnCircles();

    }
    
    public IEnumerator GenerateLetterBlocks(List<Letter> letters)
    {
        foreach (Letter letter in letters)
        {
            HangmanLetterBlock block = Instantiate(blockPrefab, blocksContainer);
            block.letterRepresented = letter.letter;
            blockObjects.Add(block);
            if (letter.isAquired)
            {
                block.GetAquired();
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

        // Destroy the circle after animation finishes
        circleSeq.OnComplete(() => Destroy(circle.gameObject));
    }
    
    void Update()
    {
        shade.transform.Rotate(0, 0 , -40f * Time.deltaTime);
    }

    public void UpdateBlock(int index)
    {
        blockObjects[index].GetAquired();
    }
    
    
}
