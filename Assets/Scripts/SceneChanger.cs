using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public void GoToGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
