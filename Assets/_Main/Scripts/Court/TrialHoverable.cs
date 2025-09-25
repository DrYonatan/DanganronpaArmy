using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialHoverable : MonoBehaviour
{
    void OnMouseEnter()
    {
        TrialCursorManager.instance.isHovering = true;
    }
    void OnMouseExit()
    {
        TrialCursorManager.instance.isHovering = false;
    }
}
