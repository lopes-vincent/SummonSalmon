using System;
using UnityEngine;

public class ScoreHolder : MonoBehaviour
{
    public float score = 0;
    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Reset()
    {
        score = 0;
    }
}
