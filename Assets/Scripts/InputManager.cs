using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject pausePanel;
    private UITrigger UITrigger;

    void Awake()
    {
        UITrigger = new UITrigger();
    }

    void OnEnable()
    {
        UITrigger.UIGame.Enable();
        UITrigger.UIGame.Pause.performed += ctx => TriggerPause();
    }

    void OnDisable()
    {
        UITrigger.UIGame.Disable();
    }

    public void TriggerPause()
    {
        if (pausePanel != null && !PersistentUI.Instance.settingsPanel.activeSelf)
        {
            pausePanel.SetActive(!pausePanel.activeSelf);
            Time.timeScale = pausePanel.activeSelf ? 0 : 1;
        }
        else
        {
            Debug.LogWarning("pausePanel is null !");
        }
    }
}
