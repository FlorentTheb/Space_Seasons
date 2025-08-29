using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform player;
    [SerializeField] private int speed;

    void Update()
    {
        if (player != null)
        {
            transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * speed);
        }
    }
}
