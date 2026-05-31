using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class User
{
    public string id;
    public string username;
    public List<SaveData> saves;
    public int score;


    public User()
    {
        
    }
    public User(string id, string username, List<SaveData> saves)
    {
        this.id = id;
        this.username = username;
        this.saves = saves;
    }

    public User(User user)
    {
        id = user.id;
        username = user.username;
        saves = user.saves;
    }
}