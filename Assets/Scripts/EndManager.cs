using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    [SerializeField]
    string apiKey;

    [SerializeField]
    private GameObject scoreForm;
    
    [SerializeField]
    private Leaderboard leaderboard;

    [SerializeField]
    private TextMeshProUGUI _scoreText;
    
    [SerializeField]
    private Score playerScoreDisplay;
    
    [SerializeField]
    private GameObject playerScoreContainer;

    [SerializeField]
    private GameObject thanksContainer;
    
    private ScoreHolder _scoreHolder;
    private List<Score> _scoreGameObjects;

    private string _playerName;

    void Awake()
    {
        _scoreHolder = GameObject.FindGameObjectWithTag("ScoreHolder").GetComponent<ScoreHolder>();
    }

    private void Start()
    {
        _scoreText.text = _scoreHolder.score.ToString();
        _scoreGameObjects = leaderboard.GetScoreGameObjects();
    }
    
    public void Restart()
    {
        _scoreHolder.Reset();
        SceneManager.LoadScene("Game");
    }
    
    public void SetPlayerName(string name)
    {
        _playerName = name;
    }
    
    public void SubmitScore()
    {
        if (_playerName.Length > 1)
        {
            scoreForm.gameObject.SetActive(false);
            thanksContainer.SetActive(true);
            StartCoroutine(PostScore(_playerName, _scoreHolder.score));
        }
    }
    
    IEnumerator PostScore(string name, float score)
    {
        SHA256 sha = SHA256.Create();
        PlayerScore playerScore = new PlayerScore();
        playerScore.uuid = Guid.NewGuid().ToString();
        playerScore.name = name;
        playerScore.score = (int)score;
        playerScore.hash = HashString(playerScore.name + playerScore.score);
        playerScore.direction = leaderboard.orderDirection;
        using (UnityWebRequest webRequest = UnityWebRequest.Post(leaderboard.apiRoute+"/summon_salmon", JsonUtility.ToJson(playerScore),  "application/json"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
                yield break;
            }

            yield return leaderboard.RefreshScores();
            
            playerScore = JsonUtility.FromJson<PlayerScore>(webRequest.downloadHandler.text);

            int playerScoreIndex = playerScore.position - 1;
            if (_scoreGameObjects.ElementAtOrDefault(playerScoreIndex))
            {
                _scoreGameObjects[playerScoreIndex].SetScore(playerScore.GetScore());
                _scoreGameObjects[playerScoreIndex].SetCreatedAt(playerScore.createdAt);
                _scoreGameObjects[playerScoreIndex].SetName(playerScore.name);
                _scoreGameObjects[playerScoreIndex].SetPosition(playerScore.position.ToString());
                _scoreGameObjects[playerScoreIndex].gameObject.SetActive(true);
                _scoreGameObjects[playerScoreIndex].HighlightText();
            }
            else
            {
                playerScoreDisplay.SetScore(playerScore.GetScore());
                playerScoreDisplay.SetCreatedAt(playerScore.createdAt);
                playerScoreDisplay.SetName(playerScore.name);
                playerScoreDisplay.SetPosition(playerScore.position.ToString());
                playerScoreDisplay.gameObject.SetActive(true);
                playerScoreContainer.SetActive(true);
            }
        }
    }
    
    public string HashString(string text)
    {
        if (String.IsNullOrEmpty(text))
        {
            return String.Empty;
        }
    
        using (var sha = new SHA256Managed())
        {
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + apiKey);
            byte[] hashBytes = sha.ComputeHash(textBytes);
        
            string hash = BitConverter
                .ToString(hashBytes)
                .Replace("-", String.Empty);

            return hash;
        }
    }
}
