using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Score;
    [SerializeField] private TextMeshProUGUI Combo;
    [SerializeField] private TextMeshProUGUI PresentsAmount;
    [SerializeField] private Image ComboRemainBar;
    [SerializeField] private Image CheckMark;

    void Start()
    {
        CheckMark.gameObject.SetActive(false);
    }
    void Update()
    {
        Score.text = $"Score :  {(int)ScoreManager.Instance.CurrentScore}";
        Combo.text = $"Combo :  X {ScoreManager.Instance.CurrentCombo}";
        PresentsAmount.text = $"{ScoreManager.Instance.TotalPresentCollected} / {ScoreManager.Instance.minimumPresentsToCollect}";
        UpdateCheckmark();
        UpdateComboBar();
    }

    private void UpdateCheckmark()
    {
        if (ScoreManager.Instance.TotalPresentCollected == ScoreManager.Instance.minimumPresentsToCollect)
            CheckMark.gameObject.SetActive(true);
        else
            CheckMark.gameObject.SetActive(false);
    }

    private void UpdateComboBar()
    {
        if (ScoreManager.Instance.CurrentCombo > 1)
            ComboRemainBar.fillAmount = ScoreManager.Instance.comboTimerRemaining / ScoreManager.Instance.ComboKeepingTimer;
        else
            ComboRemainBar.fillAmount = 0;
    }
}
