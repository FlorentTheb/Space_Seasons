using UnityEngine;

public class CheckpointSpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private GameObject CheckPointPrefab;
    private GameObject Checkpoint;
    private float checkpointSpawnZ;
    private float removeTriggerZ;
    private float speed;
    private bool isScrolling = false;

    public void Init(float initSpeed, float initSpawnTriggerZ, float initRemoveTriggerZ)
    {
        checkpointSpawnZ = initSpawnTriggerZ;
        removeTriggerZ = initRemoveTriggerZ;
        speed = initSpeed;
        isScrolling = true;
    }

    private void Start()
    {
        Vector3 initialCoordinate = new(0f, 0f, checkpointSpawnZ);

        Checkpoint = Instantiate(CheckPointPrefab, initialCoordinate, Quaternion.identity);
        Checkpoint.SetActive(false);

    }

    private void Update()
    {
        if (Checkpoint.activeSelf)
            UpdateCheckpoint();
    }

    public void SpawnCheckpoint()
    {
        Checkpoint.SetActive(true);
    }

    private void UpdateCheckpoint()
    {

        Checkpoint.transform.position += speed * Time.deltaTime * Vector3.back;
        if (Checkpoint.transform.position.z < removeTriggerZ)
            DespawnCheckpoint();
    }
    private void DespawnCheckpoint()
    {
        Checkpoint.transform.position = Vector3.forward * checkpointSpawnZ;
        Checkpoint.SetActive(false);
    }


    public void UpdateSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void StopScroll()
    {
        isScrolling = false;
    }

    public void EnableScroll()
    {
        isScrolling = true;
    }
}
