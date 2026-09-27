using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/Game Structure/VN Chapter Segment")]
public class VNChapterSegment: ChapterSegment
{
    public List<GameEvent> gameEvents;

    public override void Load()
    {
        ProgressManager.instance.gameEvents = gameEvents;
    }

    public override void LoadScene()
    {
        if(ProgressManager.instance == null)
        {
            SceneManager.LoadScene("VisualNovelCore");
            GameStateManager.instance.DestroyCurrentPersistent();
        }
    }
    
    public override string GetSceneName()
    {
        return "VisualNovelCore";
    }
    
}
