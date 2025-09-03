using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private HealthManager HM;

    private void Start()
    {
        HM = GetComponent<HealthManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Obstacle>() != null && !Player.Instance.IsInvicible)
        {
            HM.TakeDamage();
            AudioManager.Instance.PlayHit();
            ScoreManager.Instance.ResetCombo();
        }
        else if (other.GetComponent<Collectable>() != null)
        {
            other.transform.gameObject.SetActive(false);
            AudioManager.Instance.PlayCollect();
            ScoreManager.Instance.CollectPresent();
        }
        else if (other.GetComponent<Magnet>() != null && !Player.Instance.IsMagnetOn)
        {
            other.transform.gameObject.SetActive(false);
            AudioManager.Instance.PlayBonus();
            Player.Instance.ActivateMagnet();
        }
        else if (other.GetComponent<Checkpoint>() != null)
        {
            Debug.Log("Checkpoint reached");
            if (ScoreManager.Instance.TotalPresentCollected == ScoreManager.Instance.minimumPresentsToCollect)
            {
                Debug.Log("Check OK");
                AudioManager.Instance.PlayCheckpoint();
                ScoreManager.Instance.IncreaseCollectablesAmountRequired();
            }
            else
            {
                Debug.Log("Check failed, Game Over !");
                SceneController.Instance.LoadGameOver();
            }
        }
    }
}
