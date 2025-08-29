using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField] private float DistanceMultiplyer = .02f;
    private float CurrentSpeed;
    public float CurrentScore { get; private set; }
    public int CurrentCombo { get; private set; }
    private int[] ComboThreshold = new int[] { 5, 15, 30, 50 };
    private int currentComboIndex;
    public int TotalPresentCollected { get; private set; }
    public int minimumPresentsToCollect;
    private int PresentCollectedStreak;
    public readonly float ComboKeepingTimer = 2f;
    public float comboTimerRemaining;

    void Awake()
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
    void Start()
    {
        minimumPresentsToCollect = 20;
        comboTimerRemaining = ComboKeepingTimer;
        CurrentScore = 0;
        CurrentCombo = 1;
        TotalPresentCollected = 0;
        PresentCollectedStreak = 0;
        currentComboIndex = 0;
    }
    void Update()
    {
        CurrentScore += CurrentSpeed * Time.deltaTime * CurrentCombo * DistanceMultiplyer;
        if (CurrentCombo > 1)
        {
            comboTimerRemaining -= Time.deltaTime;
            if (comboTimerRemaining <= 0f)
            {
                DecreaseCombo();
                comboTimerRemaining = ComboKeepingTimer;
            }
        }
    }

    public void AddNewScore()
    {
        List<int> bestScores = SaveSystem.LoadBestScores();

        bestScores.Add((int)CurrentScore);

        bestScores.Sort((a, b) => b.CompareTo(a));

        if (bestScores.Count > 5)
            bestScores.RemoveRange(5, bestScores.Count - 5);

        SaveSystem.SaveBestScores(bestScores);
    }

    public void ResetCombo()
    {
        CurrentCombo = 1;
        PresentCollectedStreak = 0;
        currentComboIndex = 0;
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        CurrentCombo = 1;
        TotalPresentCollected = 0;
        PresentCollectedStreak = 0;
        currentComboIndex = 0;
        minimumPresentsToCollect = 20;
    }

    public void CollectPresent()
    {
        PresentCollectedStreak++;
        if (TotalPresentCollected < minimumPresentsToCollect)
            TotalPresentCollected++;
        comboTimerRemaining = ComboKeepingTimer;
        CheckNextCombo();
    }

    public void IncreaseCollectablesAmountRequired()
    {
        minimumPresentsToCollect += 10;
        minimumPresentsToCollect = Mathf.Clamp(minimumPresentsToCollect, 20, 80);
        TotalPresentCollected = 0;
        ResetCombo();
    }


    private void DecreaseCombo()
    {
        CurrentCombo--;
        currentComboIndex--;
        PresentCollectedStreak = 0;
    }

    private void CheckNextCombo()
    {
        if (PresentCollectedStreak >= ComboThreshold[currentComboIndex] && CurrentCombo < 5)
        {
            CurrentCombo++;
            if (currentComboIndex < ComboThreshold.Length - 1)
            {
                currentComboIndex++;
            }
        }
    }

    public void UpdateCurrentSpeed(float newSpeed)
    {
        CurrentSpeed = newSpeed;
    }
}
