using System.Collections;
using UnityEngine;

public class FillFullHPCommand : Command
{
    public override IEnumerator Execute()
    {
        TrialManager.instance.barsAnimator.ShowGlobalBars(0.2f);
        TrialManager.instance.IncreaseHealth(TrialManager.instance.playerStats.maxHP);
        yield return new WaitForSeconds(2f);
    }

#if UNITY_EDITOR
    public void DrawGUI()
    {
        base.DrawGUI();
    }
#endif
}