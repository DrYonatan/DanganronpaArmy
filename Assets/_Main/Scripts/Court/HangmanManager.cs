using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HangmanManager : MonoBehaviour
{
    public HangmansGambit game;
    public List<HangmanLetter> letters = new List<HangmanLetter>();
    public HangmanLetter letterPrefab;
    public AudioClip music;

    void Start()
    {
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

    void SpawnLetter(char c)
    {
        HangmanLetter letter = Instantiate(letterPrefab, transform);
        letter.letter = c;
        RectTransform parentRect = transform as RectTransform;

        float randomX = Random.Range(-parentRect.rect.width / 3f, parentRect.rect.width / 3f);
        float randomY = Random.Range(-parentRect.rect.height / 3f, parentRect.rect.height / 3f);

        letter.GetComponent<RectTransform>().anchoredPosition = new Vector2(randomX, randomY);
        letters.Add(letter);
        letter.canvasGroup.DOFade(0f, 0.5f)
            .SetDelay(3f) // wait 3 seconds before fading
            .OnComplete(() =>
            {
                letters.Remove(letter);
                Destroy(letter.gameObject);
            });
    }
}
