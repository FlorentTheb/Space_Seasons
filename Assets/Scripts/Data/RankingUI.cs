using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BestScores : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI BestScore;
    void Update()
    {
        DisplayBestScores();
    }
    void DisplayBestScores()
    {
        List<int> bestScores = SaveSystem.LoadBestScores();
        string text = "";
        for (int i = 0; i < bestScores.Count; i++)
        {
            text += $"{i + 1}. {bestScores[i]}\n";
        }

        BestScore.text = text;
    }
}
