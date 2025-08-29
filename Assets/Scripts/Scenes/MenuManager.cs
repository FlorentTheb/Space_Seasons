using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject panelRanking;
    public GameObject panelCredits;

    public void TogglePlay()
    {
        SceneController.Instance.RestartGame();
    }

    public void ToggleRanking()
    {
        panelRanking.SetActive(!panelRanking.activeSelf);
    }

    public void ToggleCredits()
    {
        panelCredits.SetActive(!panelCredits.activeSelf);
    }

    public void ToggleSettings()
    {
        PersistentUI.Instance.ToggleSettingsPanel();
    }

    public void QuitGame()
    {
        Debug.Log("Leaving the game ...");
        Application.Quit();
    }
}
