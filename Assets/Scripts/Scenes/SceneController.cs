using System.Collections;
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

    public void LoadScene(string sceneName, int? musicIndex = null)
    {
        StartCoroutine(LoadSceneAndMusic(sceneName, musicIndex));
    }

    private IEnumerator LoadSceneAndMusic(string sceneName, int? musicIndex)
    {
        var asyncOp = SceneManager.LoadSceneAsync(sceneName);
        asyncOp.allowSceneActivation = true;

        while (!asyncOp.isDone)
            yield return null;

        if (musicIndex.HasValue)
            AudioManager.Instance.CrossfadeMusic(musicIndex.Value);
    }

    public void LoadGameOver()
    {
        LoadScene("GameOver");
    }

    public void LoadMainMenu()
    {
        LoadScene("Menu", 0);
    }

    public void RestartGame()
    {
        ScoreManager.Instance.ResetScore();
        LoadScene("Game", 1);
    }
}
