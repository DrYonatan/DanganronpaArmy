using System;
using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Court;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class Letter
{
    public char letter;
    public bool isAquired;
}

[CreateAssetMenu(menuName = "Data/Hangmans Gambit")]
public class HangmansGambit : TrialSegment
{
    public bool isActive = true;
    public char[] possibleLetters;
    public List<Letter> correctLetters = new List<Letter>();

    public override void Finish()
    {
        base.Finish();
        HangmanManager.instance.gameObject.SetActive(false);
    }

    public override void Play()
    {
        HangmanManager.instance.Play(this);
    }

}
