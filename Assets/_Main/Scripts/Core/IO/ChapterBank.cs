
using System.Collections.Generic;
using UnityEngine;

public class ChapterBank : MonoBehaviour
{
    public static ChapterBank instance {get; private set;}
    
    public List<Chapter> chapters;

    void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
        }
    }
}
