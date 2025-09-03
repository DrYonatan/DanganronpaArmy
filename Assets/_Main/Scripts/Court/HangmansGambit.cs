using System.Collections;
using System.Collections.Generic;
using _Main.Scripts.Court;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Hangmans Gambit")]
public class HangmansGambit : TrialSegment
{
    public bool isActive = true;
    public char[] possibleLetters;

    public override void Play()
    {
        
    }

}
