using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;

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
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("userId", userId);
        data.Add("username", username);
        yield return HttpRequestUtils.PostRequest<User>($"{SERVER_ADDRESS}/user", data,
            (response) => { loggedInUser = response; });
    }

    public void UpdateCloudSave(int slot, SaveData saveData)
    {
        StartCoroutine(UpdateCloudSaveRequest(slot, saveData));
    }

    IEnumerator UpdateCloudSaveRequest(int slot, SaveData saveData)
    {
        yield return HttpRequestUtils.PostRequest<SaveData>($"{SERVER_ADDRESS}/saves/{loggedInUser.id}/{slot}",
            saveData,
            (response) =>
            {
                loggedInUser.saves[slot] = response;
            });
    }
}