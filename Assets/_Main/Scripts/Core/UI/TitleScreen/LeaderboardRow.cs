using TMPro;
using UnityEngine;

public class LeaderboardRow: MonoBehaviour
{
    public TextMeshProUGUI ranking;
    public TextMeshProUGUI username;
    public TextMeshProUGUI score;

    public void UpdateValues(int ranking, string username, int score)
    {
        this.ranking.text = $"{ranking}#";
        this.username.text = username;
        this.score.text = score.ToString();
    }
}