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
        if (other.GetComponent<Obstacle>() != null)
        {
            HM.TakeDamage();
            ScoreManager.Instance.ResetCombo();
        }
        else if (other.GetComponent<Collectable>() != null)
        {
            other.transform.gameObject.SetActive(false);
            ScoreManager.Instance.CollectPresent();
        }
        else if (other.GetComponent<Magnet>() != null && !Player.Instance.IsMagnetOn)
        {
            other.transform.gameObject.SetActive(false);
            Player.Instance.ActivateMagnet();
        }
        else if (other.GetComponent<Checkpoint>() != null)
        {
            if (ScoreManager.Instance.TotalPresentCollected == ScoreManager.Instance.minimumPresentsToCollect)
            {
                ScoreManager.Instance.IncreaseCollectablesAmountRequired();
            }
            else
                SceneController.Instance.LoadGameOver();
        }
    }
}
