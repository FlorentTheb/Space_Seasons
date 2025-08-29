using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class GroundSpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private GameObject GroundPrefab;
    private readonly Queue<GameObject> grounds = new();
    private readonly int groundNumber = 3;
    private readonly float groundLength = 120f;
    private float removeTriggerZ;
    private float speed;
    private bool isScrolling = false;

    public void Init(float initSpeed, float initSpawnTriggerZ, float initRemoveTriggerZ)
    {
        removeTriggerZ = initRemoveTriggerZ;
        speed = initSpeed;
        isScrolling = true;
    }

    private void Start()
    {
        for (int i = 0; i < groundNumber; i++)
        {
            GameObject newGround = Instantiate(GroundPrefab, Vector3.forward * (i * groundLength), Quaternion.identity);
            grounds.Enqueue(newGround);
        }
    }

    private void Update()
    {
        if (isScrolling)
            UpdateGrounds();
    }

    private void SpawnGround()
    {
        GameObject oldGround = grounds.Dequeue();

        float newestZCoordinate = grounds.Last().transform.position.z;
        float newZCoordinate = newestZCoordinate + groundLength;

        oldGround.transform.position = Vector3.forward * newZCoordinate;

        grounds.Enqueue(oldGround);
    }

    private void UpdateGrounds()
    {
        foreach (var ground in grounds)
            ground.transform.position += Vector3.back * Time.deltaTime * speed;

        if (grounds.First().transform.position.z < removeTriggerZ)
            SpawnGround();
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
