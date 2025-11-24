using System.Collections.Generic;
using UnityEngine;

public class WorldCharactersParent : MonoBehaviour
{
    public List<WorldCharacter> worldCharacters = new List<WorldCharacter>();

    public void AnimateCharacters()
    {
        foreach (WorldCharacter worldCharacter in worldCharacters)
        {
            worldCharacter.AppearAnimation();
        }
    }
}
