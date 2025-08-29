using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public GameObject pausePanel;

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenSettings()
    {
        PersistentUI.Instance.ToggleSettingsPanel();
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1;
        SceneController.Instance.LoadMainMenu();
    }
}
