using System.Collections.Generic;
using UnityEngine;

public class RoomModel : MonoBehaviour
{
    public List<RoomIntroEffect> roomIntroEffects = new List<RoomIntroEffect>();

    public void PlayRoomIntroEffects()
    {
        foreach (RoomIntroEffect introEffect in roomIntroEffects)
        {
            StartCoroutine(introEffect.PlayEffect());
        }
    }
}
