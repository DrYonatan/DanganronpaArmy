using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public string userId;
    public string username;

    public UserData(string userId, string username)
    {
        this.userId = userId;
        this.username = username;
    }
}

public class Score
{
    public int score;

    public Score(int score)
    {
        this.score = score;
    }
}

public class UserDataManager : MonoBehaviour, IAuthenticationListener
{
    const string SERVER_ADDRESS = "http://localhost:3000/api";
    public User loggedInUser;
    public static UserDataManager instance { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        FirebaseUser user = FirebaseManager.instance.user;
        if (user != null)
        {
            StartCoroutine(FetchUserData(user.UserId));
        }

        FirebaseManager.instance.AddAuthenticationListener(this);
    }

    public void OnAuthentication()
    {
        FirebaseUser user = FirebaseManager.instance.user;
        if (user != null)
        {
            StartCoroutine(FetchUserData(user.UserId));
        }
        else
        {
            loggedInUser = null;
        }
    }

    public void OnSignup(string userId, string username)
    {
        StartCoroutine(CreateNewUser(userId, username));
    }

    IEnumerator FetchUserData(string userId)
    {
        yield return HttpRequestUtils.GetRequest<User>($"{SERVER_ADDRESS}/users/{userId}",
            (user) => { loggedInUser = new User(user); });
    }

    IEnumerator CreateNewUser(string userId, string username)
    {
        UserData data = new UserData(userId, username);
        yield return HttpRequestUtils.PostRequest<User>($"{SERVER_ADDRESS}/user", data,
            (response) => { loggedInUser = response; });
    }

    public void UpdateCloudSave(int slot, SaveData saveData, Action onComplete)
    {
        StartCoroutine(UpdateCloudSaveRequest(slot, saveData, onComplete));
    }

    IEnumerator UpdateCloudSaveRequest(int slot, SaveData saveData, Action onComplete)
    {
        yield return HttpRequestUtils.PostRequest<SaveData>($"{SERVER_ADDRESS}/saves/{loggedInUser.id}/{slot}",
            saveData,
            (response) =>
            {
                for (int i = loggedInUser.saves.Count - 1; i < slot; i++)
                {
                    loggedInUser.saves.Add(null);
                }

                loggedInUser.saves[slot] = response;
                onComplete();
            });
    }

    public void UpdateUserScore(int score)
    {
        loggedInUser.score += score;
        PostScore();
    }

    void PostScore()
    {
        Score score = new Score(loggedInUser.score);
        PopupAnimator.instance?.ShowScorePopup(loggedInUser.score);
        StartCoroutine(PostScoreRequest(score));
    }

    IEnumerator PostScoreRequest(Score score)
    {
        yield return HttpRequestUtils.PostRequest<Score>($"{SERVER_ADDRESS}/userscores/{loggedInUser.id}",
            score,
            (response) => { });
    }
}