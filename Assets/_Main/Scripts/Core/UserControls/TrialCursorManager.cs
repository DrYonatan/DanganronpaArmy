using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialCursorManager : CursorManager
{
    private Vector3 originalScale = Vector3.one;
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1.1f);
    
    protected override void ManageHover()
    {
        Vector3 goalScale = originalScale;
        int actualSpeed = speed;

        if(isHovering)
        {
            actualSpeed = 0;
            goalScale = hoverScale;  
        }
        cursor.localScale = Vector3.Lerp(cursor.localScale, goalScale, Time.unscaledDeltaTime * 20f);
        reticle.Rotate(0, 0, actualSpeed * Time.unscaledDeltaTime);

    }
}
