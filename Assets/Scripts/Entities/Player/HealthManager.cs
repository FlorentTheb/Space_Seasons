using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Image HealthAmountImage;
    [SerializeField] private float MaxHealthPoints = 3;
    private float lerpSpeed;
    private float CurrentHealth;
    void Start()
    {
        CurrentHealth = MaxHealthPoints;
    }

    void Update()
    {
        lerpSpeed = 4f * Time.deltaTime;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        HealthAmountImage.fillAmount = Mathf.Lerp(HealthAmountImage.fillAmount, CurrentHealth / MaxHealthPoints, lerpSpeed);
    }

    public void TakeDamage()
    {
        CurrentHealth--;
        Player.Instance.PlayerDamaged();
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            SceneController.Instance.LoadGameOver();
        }
    }

    public void Heal()
    {
        if (CurrentHealth < MaxHealthPoints)
        {
            CurrentHealth++;
        }
    }

    public void ResetHealth()
    {
        CurrentHealth = MaxHealthPoints;
    }
}
