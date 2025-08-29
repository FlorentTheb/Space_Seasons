using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void LoadGameOver()
    {
        LoadScene("GameOver");
    }

    public void LoadMainMenu()
    {
        LoadScene("Menu");
        AudioManager.Instance.CrossfadeMusic(0);
    }

    public void RestartGame()
    {
        ScoreManager.Instance.ResetScore();
        AudioManager.Instance.CrossfadeMusic(1);
        LoadScene("Game");
    }
}
