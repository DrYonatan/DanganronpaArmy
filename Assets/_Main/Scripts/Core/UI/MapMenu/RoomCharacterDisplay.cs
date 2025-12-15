using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomCharacterDisplay : MonoBehaviour
{
    public Image characterSprite;

    public void SetCharacterSprite(Sprite sprite)
    {
        characterSprite.sprite = sprite;
    }
}
