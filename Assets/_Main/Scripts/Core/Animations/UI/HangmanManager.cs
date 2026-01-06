using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DG.Tweening;
using DIALOGUE;
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
    public bool isActive = true;
    
    private Vector3 originalCameraPosition;

    void Awake()
    {
        instance = this;
    }
    public void Play(HangmansGambit game)
    {
        this.game = game;
        isActive = false;
        MusicManager.instance.PlaySong(music);
        StartCoroutine(StartGame());
    }


    private void SetCharacter()
    {
        CharacterStand characterStand = TrialManager.instance.characterStands.Find(stand => stand.character == game.character);
        CharacterState concentrating = game.character.FindStateByName("concentrating");
        if (concentrating != null)
        {
            characterStand.SetSprite(concentrating);
        }
        CameraController.instance.TeleportToTarget(characterStand.transform, characterStand.heightPivot, new Vector3(0, -0.2f, -3.5f), Vector3.zero, 0);
        animator.SetSilhouette(characterStand);

    }

    private void ActivateGame()
    {
        animator.gameObject.SetActive(true);
        animator.canvasGroup.alpha = 0f;
        letterIndex = 0;
        animator.ShowHangmanUI();
        isActive = true;
        TimeManipulationManager.instance.isInputActive = true;
        TimerManager.instance.SetTimer(600);
        CursorManager.instance.Show();
    }
    IEnumerator StartGame()
    {
        yield return CameraController.instance.DiscussionOutroMovement(2.5f);
        SetCharacter();
        ImageScript.instance.UnFadeToBlack(1f);
        MinigameStartAnimation startAnimation = Instantiate(animator.startAnimation, TrialManager.instance.globalUI);
        startAnimation.Animate(0.5f);
        yield return CameraController.instance.MoveAndRotate(new Vector3(0, 0, 1f), Vector3.zero, 2.5f);
        ActivateGame();
        yield return animator.GenerateLetterBlocks(game.correctLetters);
        CheckAquiredLetters();
        StartCoroutine(SpawnLetters(game.possibleLetters));
        MoveCameraAway();
    }

    private void MoveCameraAway()
    {
        originalCameraPosition = CameraController.instance.cameraTransform.position;
        CameraController.instance.cameraTransform.position = new Vector3(1000, 1000, 1000); // Teleport far, far away
    }

    void Update()
    {
        if (game != null && isActive)
        {
            CursorManager.instance.ReticleAsCursor();
            animator.UpdateTimerText();
        }
    }
    
    IEnumerator SpawnLetters(char[] chars)
    {
        while (isActive)
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
            TrialManager.instance.DecreaseHealthDefault(1f);
            if (TrialManager.instance.playerStats.hp <= 0)
            {
                StartCoroutine(GameOverPipeline());
            }
        }
    }

    private IEnumerator GameOverPipeline()
    {
        StopGame();
        TrialManager.instance.barsAnimator.HideGlobalBars(0f);
        TrialCursorManager.instance.Hide();
        yield return new WaitForSeconds(0.5f);
        yield return TrialManager.instance.ShowFailedScreen();
        animator.transform.DOKill();
        MusicManager.instance.StopSong();
        animator.gameObject.SetActive(false);
        ImageScript.instance.UnFadeToBlack(0.2f);
        yield return CameraController.instance.FovOutro();
        StartCoroutine(TrialManager.instance.GameOver());
    }

    private void ProceedToNextLetter()
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
        CameraController.instance.cameraTransform.position = originalCameraPosition;
        StopGame();
        StartCoroutine(FinishPipeline());
    }

    private void StopGame()
    {
        TimeManipulationManager.instance.DeActivateInput();
        isActive = false;
        animator.HideLetterObjects();
    }
    

    IEnumerator FinishPipeline()
    {
        yield return animator.FinishAnimation();
        animator.canvasGroup.DOFade(0f, 0f);
        yield return new WaitForSeconds(0.1f);
        ImageScript.instance.UnFadeToBlack(0.5f);
        yield return CameraController.instance.FovOutro();
        MusicManager.instance.StopSong();
        animator.transform.DOKill();
        game.Finish();
    }

    void SpawnLetter(char c)
    {
        RectTransform parentRect = animator.letterObjectsContainer;

        HangmanLetter letter = Instantiate(letterPrefab, parentRect);
        letter.transform.SetAsFirstSibling();
        letter.letter = c;

        float randomX = Random.Range(-parentRect.rect.width, parentRect.rect.width);
        float randomY = Random.Range(-parentRect.rect.height, parentRect.rect.height);

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
