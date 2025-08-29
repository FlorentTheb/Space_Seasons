using UnityEngine;

public class Collectable : MonoBehaviour
{
    public bool isMagnet = false;
    [SerializeField] private float MagnetSpeed = 10f;
    void Update()
    {
        if (isMagnet)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(Player.Instance.transform.position.x, 4, Player.Instance.transform.position.z), MagnetSpeed * Time.deltaTime);
        }
    }
}
