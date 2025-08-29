using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameOverUI : MonoBehaviour
{
    public TMP_Text gameOverMessage;
    public TMP_Text currentScoreText;
    public TMP_Text bestScoreText;

    void Start()
    {
        int score = (int)ScoreManager.Instance.CurrentScore;
        List<int> bestScores = SaveSystem.LoadBestScores();

        string message;
        currentScoreText.text = "Score : " + score.ToString();

        if (bestScores.Count == 0 || score > bestScores[0])
        {
            message = "Tu as battu le record !";
            bestScoreText.text = "Meilleur score : " + score.ToString();
        }
        else if (bestScores.Count < 5 || score > bestScores[bestScores.Count - 1])
        {
            message = "Bravo, tu es dans le top 5 !";
            bestScoreText.text = "Meilleur score : " + bestScores[0].ToString();
        }
        else
        {
            message = "Tu feras mieux la prochaine fois !";
            bestScoreText.text = "Meilleur score : " + bestScores[0].ToString();
        }

        ScoreManager.Instance.AddNewScore();

        gameOverMessage.text = message;
    }

    public void OnClickMenu()
    {
        SceneController.Instance.LoadMainMenu();
    }

    public void OnClickPlay()
    {
        SceneController.Instance.RestartGame();
    }
}
