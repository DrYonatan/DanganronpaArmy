using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class HangmanManager : MonoBehaviour
{
    public static HangmanManager instance { get; private set; }

    public HangmanUIAnimator animator;

    public HangmansGambit game;
    public List<HangmanLetter> letterObjects = new List<HangmanLetter>();
    public HangmanLetter letterPrefab;
    public AudioClip music;
    public int letterIndex = 0;

    public float timeLeft = 600f;
    

    void Awake()
    {
        instance = this;
    }
    public void Play(HangmansGambit game)
    {
        this.game = game;
        game.isActive = false;
        MusicManager.instance.PlaySong(music);
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        CharacterStand characterStand = TrialManager.instance.characterStands.Find(stand => stand.character == game.character);
        yield return CameraController.instance.DiscussionOutroMovement(2.5f);
        ImageScript.instance.UnFadeToBlack(1f);
        CameraController.instance.TeleportToTarget(characterStand.transform, characterStand.heightPivot, new Vector3(0, -0.2f, -3.5f), Vector3.zero, 0);
        yield return CameraController.instance.MoveAndRotate(new Vector3(0, 0, 0.5f), Vector3.zero, 1.5f);
        animator.gameObject.SetActive(true);
        animator.canvasGroup.alpha = 0f;
        animator.SetSilhouette(characterStand);
        animator.ShowHangmanUI();
        game.isActive = true;
        animator.canvasGroup.DOFade(1f, 0.5f);
        CursorManager.instance.Show();
        yield return animator.GenerateLetterBlocks(game.correctLetters);
        CheckAquiredLetters();
        StartCoroutine(SpawnLetters(game.possibleLetters));
    }

    void Update()
    {
        if (game != null && game.isActive)
        {
            CursorManager.instance.ReticleAsCursor();
            timeLeft -= Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeLeft);
            animator.UpdateTimerText(timeSpan);
        }
    }
    
    IEnumerator SpawnLetters(char[] chars)
    {
        while (game.isActive)
        {
            // Spawn a letter
            int randomInt = Random.Range(0, chars.Length);
            SpawnLetter(chars[randomInt]);
            float waitTime = Random.Range(1f, 3f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void CheckLetter(char letter)
    {
        if (game.correctLetters[letterIndex].letter == letter)
        {
            game.correctLetters[letterIndex].isAquired = true;
            animator.AquireBlock(letterIndex);
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
        for (int i = letterIndex; i < game.correctLetters.Count; i++)
        {
            if (game.correctLetters[i].isAquired)
                letterIndex++;
        }
        if (letterIndex >= game.correctLetters.Count)
        {
            FinishGame();
        }
        else
        {
            animator.TurnBlockIntoCurrent(letterIndex);
        }
        
    }

    void CheckAquiredLetters()
    {
        for (int i = letterIndex; i < game.correctLetters.Count; i++)
        {
            if (game.correctLetters[i].isAquired)
                letterIndex++;
            else
            {
                animator.TurnBlockIntoCurrent(letterIndex);
                return;
            }
        }
    }

    void FinishGame()
    {
        game.isActive = false;
        HideAllLetterObjects();
        StartCoroutine(FinishPipeline());
    }
    
    public void HideAllLetterObjects()
    {
        foreach (HangmanLetter letter in letterObjects)
        {
            letter.canvasGroup.DOKill();
            letter.canvasGroup.DOFade(0f, 0.5f);
        }
    }

    IEnumerator FinishPipeline()
    {
        yield return animator.FinishAnimation();
        animator.canvasGroup.DOFade(0f, 0f);
        ImageScript.instance.FadeToBlack(0.01f);
        yield return new WaitForSeconds(0.01f);
        ImageScript.instance.UnFadeToBlack(0.5f);
        StartCoroutine(CameraController.instance.ChangeFov(25f, 1.5f));
        yield return CameraController.instance.MoveAndRotate(new Vector3(0f, 0f, 2f), new Vector3(0f, 0f, 0f), 1.5f);
        MusicManager.instance.StopSong();
        game.Finish();
    }

    void SpawnLetter(char c)
    {
        HangmanLetter letter = Instantiate(letterPrefab, animator.transform);
        letter.letter = c;
        RectTransform parentRect = animator.transform as RectTransform;

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
