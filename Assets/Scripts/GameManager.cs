using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{ 
    [Range(1, 100)]
    public float speedFactor = 1f;
    
    public List<float> scoreSpeedUps = new List<float>();
    
    public ScoreHolder scoreHolder;
    public TextMeshProUGUI scoreValueText;

    public AudioSource music;

    public GameObject pauseMenu;
    
    public bool _stopTimer = false;
    
    private void Start()
    {
        StartCoroutine(ScoreCounter());
    }
    
    private void Update()
    {
        music.pitch = Mathf.Clamp(speedFactor * 0.3f, 1, 3f);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    IEnumerator ScoreCounter()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            if (!_stopTimer)
            {
                scoreHolder.score += 10f;
                scoreValueText.text = scoreHolder.score.ToString();
                if (scoreSpeedUps.Contains(scoreHolder.score))
                {
                    speedFactor += 0.7f;
                }
            }
        }
    }
    
    public void Restart()
    {
        _stopTimer = false;
        scoreHolder.Reset();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
    
    public void TogglePause()
    {
        _stopTimer = !_stopTimer;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
    }
}
