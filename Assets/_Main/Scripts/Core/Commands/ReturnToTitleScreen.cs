using System.Collections;
using UnityEngine;

public class ReturnToTitleScreen: Command
{
    public override IEnumerator Execute()
    {
        GameStateManager.instance.StartCoroutine(GameStateManager.instance.GoToTitleScreenPipeline());
        yield return new WaitForSeconds(0.5f);
    }
}