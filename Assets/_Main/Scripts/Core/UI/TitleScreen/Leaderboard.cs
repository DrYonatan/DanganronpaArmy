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
    private const float SCROLL_THRESHOLD = 0.1f;
    public ScrollRect scrollRect;
    public float speed = 1f;

    private bool isFetching = false;
    private bool hasMoreData = true;

    void OnEnable()
    {
        userRow.gameObject.SetActive(false);
        scrollRect.onValueChanged.AddListener(OnScroll);
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
        isFetching = true;
        yield return HttpRequestUtils.GetRequest<List<UserScore>>($"{SERVER_ADDRESS}/userscores/{startIndex}/{endIndex}",
            (scores) =>
            {
                isFetching = false;

                if (scores == null || scores.Count < BATCH_AMOUNT)
                    hasMoreData = false;

                int previousCount = userScores.Count;
                userScores.AddRange(scores);
                UpdateTable(previousCount);
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
        if (!hasMoreData || isFetching)
            return;

        StartCoroutine(FetchScores(userScores.Count, userScores.Count + BATCH_AMOUNT));
    }

    void OnScroll(Vector2 scrollPosition)
    {
        if (scrollRect.verticalNormalizedPosition <= SCROLL_THRESHOLD)
            FetchMoreScores();
    }

    void UpdateTable(int startIndex = 0)
    {
        for (int i = startIndex; i < userScores.Count; i++)
        {
            UserScore scoring = userScores[i];
            LeaderboardRow row = Instantiate(leaderboardRowPrefab, container);
            rows.Add(row.gameObject);
            row.UpdateValues(i + 1, scoring.username, scoring.score);
        }

        if (startIndex == 0)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 1f;

            User user = UserDataManager.instance.loggedInUser;
            if (user != null)
                StartCoroutine(FetchUserRank(user));
        }
    }

    void OnDisable()
    {
        scrollRect.onValueChanged.RemoveListener(OnScroll);

        foreach (GameObject row in rows)
        {
            Destroy(row);
        }

        rows.Clear();
        userScores.Clear();
        isFetching = false;
        hasMoreData = true;
    }
}