using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    [Range(1, 100)]
    public float speedFactor = 1f;
    
    public List<float> scoreSpeedUps = new List<float>();
    
    public float scoreValue = 0f;
    public TextMeshProUGUI scoreValueText;

    public AudioSource music;

    private void Start()
    {
        StartCoroutine(ScoreCounter());
    }
    
    private void Update()
    {
        music.pitch = Mathf.Clamp(speedFactor * 0.3f, 1, 3f);
    }

    IEnumerator ScoreCounter()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            scoreValue += 10f;
            scoreValueText.text = scoreValue.ToString();
            if (scoreSpeedUps.Contains(scoreValue))
            {
                speedFactor += 0.7f;
            }
        }
    }
}
