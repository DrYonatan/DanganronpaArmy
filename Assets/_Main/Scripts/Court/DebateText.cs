using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebateText : MonoBehaviour
{
    void OnMouseEnter()
    {
        GameLoop.instance.currentAimedText = this;
    }
    void OnMouseExit()
    {
        GameLoop.instance.currentAimedText = null;
    }
}
