using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserScore
{
    public string id;
    public string username;
    public int score;
}

public class Leaderboard : TitleScreenSubMenu
{
    public LeaderboardRow leaderboardRowPrefab;
    public LeaderboardRow userRow;
    public List<UserScore> userScores = new List<UserScore>();
    public Transform container;
    public List<GameObject> rows = new List<GameObject>();
    const string SERVER_ADDRESS = "http://localhost:3000/api";
    private const int BATCH_AMOUNT = 20;
    public ScrollRect scrollRect;
    public float speed = 1f;

    void OnEnable()
    {
        userRow.gameObject.SetActive(false);
        StartCoroutine(FetchScores(0, BATCH_AMOUNT));
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            scrollRect.verticalNormalizedPosition += speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            scrollRect.verticalNormalizedPosition -= speed * Time.deltaTime;
        }

        scrollRect.verticalNormalizedPosition =
            Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
    }

    IEnumerator FetchScores(int startIndex, int endIndex)
    {
        yield return HttpRequestUtils.GetRequest<List<UserScore>>($"{SERVER_ADDRESS}/userscores/{startIndex}/{endIndex}",
            (scores) =>
            {
                userScores = scores;
                UpdateTable();
            });
    }

    IEnumerator FetchUserRank(User user)
    {
        yield return HttpRequestUtils.GetRequest<int>($"{SERVER_ADDRESS}/ranking/{user.id}", (ranking) =>
        {
            userRow.gameObject.SetActive(true);
            userRow.UpdateValues(ranking, user.username, user.score);
        });
    }

    void FetchMoreScores()
    {
        StartCoroutine(FetchScores(rows.Count, rows.Count + BATCH_AMOUNT));
    }

    void OnScroll()
    {
        FetchMoreScores();
    }

    void UpdateTable()
    {
        for (int i = 0; i < userScores.Count; i++)
        {
            UserScore scoring = userScores[i];
            LeaderboardRow row = Instantiate(leaderboardRowPrefab, container);
            rows.Add(row.gameObject);
            row.UpdateValues(i + 1, scoring.username, scoring.score);
        }

        User user = UserDataManager.instance.loggedInUser;
        if (user != null)
        {
            StartCoroutine(FetchUserRank(user));
        }

    }

    void OnDisable()
    {
        foreach (GameObject row in rows)
        {
            Destroy(row);
        }

        rows.Clear();
        userScores.Clear();
    }
}