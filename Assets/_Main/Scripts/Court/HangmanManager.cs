using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HangmanManager : MonoBehaviour
{
    public static HangmanManager instance { get; private set; }

    public HangmanUIAnimator animator;

    public HangmansGambit game;
    public List<HangmanLetter> letterObjects = new List<HangmanLetter>();
    public HangmanLetter letterPrefab;
    public AudioClip music;
    public int letterIndex = 0;
    

    void Awake()
    {
        instance = this;
    }
    public void Play(HangmansGambit game)
    {
        this.game = game;
        StartCoroutine(animator.GenerateLetterBlocks(game.correctLetters));
        StartCoroutine(SpawnLetters(game.possibleLetters));
        MusicManager.instance.PlaySong(music);
    }

    void Update()
    {
        CursorManager.instance.ReticleAsCursor();
    }
    
    IEnumerator SpawnLetters(char[] chars)
    {
        while (game.isActive)
        {
            float waitTime = Random.Range(1f, 3f);
            yield return new WaitForSeconds(waitTime);

            // Spawn a letter
            int randomInt = Random.Range(0, chars.Length);
            SpawnLetter(chars[randomInt]);
        }
    }

    public void CheckLetter(char letter)
    {
        if (game.correctLetters[letterIndex].letter == letter)
        {
            game.correctLetters[letterIndex].isAquired = true;
            animator.UpdateBlock(letterIndex);
            ProceedToNextLetter();
        }
        else
        {
            Debug.Log("Wrong! Correct Letter: " + game.correctLetters[letterIndex].letter);
            // TODO make player lose HP if they got it wrong
        }
    }

    void ProceedToNextLetter()
    {
        letterIndex++;
        if (letterIndex >= game.correctLetters.Count)
        {
            FinishGame();
        }
        for (int i = letterIndex; i < game.correctLetters.Count; i++)
        {
            if (game.correctLetters[i].isAquired)
                letterIndex++;
            else
            {
                return;
            }
        }
    }

    void FinishGame()
    {
        game.isActive = false;
        StartCoroutine(FinishPipeline());
    }

    IEnumerator FinishPipeline()
    {
        yield return animator.FinishAnimation();
        game.Finish();
    }

    void SpawnLetter(char c)
    {
        HangmanLetter letter = Instantiate(letterPrefab, transform);
        letter.letter = c;
        RectTransform parentRect = transform as RectTransform;

        float randomX = Random.Range(-parentRect.rect.width / 3f, parentRect.rect.width / 3f);
        float randomY = Random.Range(-parentRect.rect.height / 3f, parentRect.rect.height / 3f);

        letter.GetComponent<RectTransform>().anchoredPosition = new Vector2(randomX, randomY);
        letterObjects.Add(letter);
        letter.canvasGroup.DOFade(0f, 0.5f)
            .SetDelay(3f) // wait 3 seconds before fading
            .OnComplete(() =>
            {
                letterObjects.Remove(letter);
                letter.Kill();
            });
    }
}
