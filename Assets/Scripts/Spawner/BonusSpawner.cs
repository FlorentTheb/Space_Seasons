using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private GameObject BarleyPrefab;
    private GameObject BarleyInstanciated;
    private readonly List<GameObject> AsleepBonusList = new();
    private readonly List<GameObject> ActiveBonusList = new();
    private float bonusSpawnZ;
    public float ObstacleSpawnTimerTreshold { get; private set; } = 2f;
    public float CurrentTimer { get; private set; } = 0f;
    private float removeTriggerZ;
    private float speed;
    private bool isScrolling = false;
    private bool HasBonusSpawned = false;


    public void Init(float initSpeed, float initSpawnTriggerZ, float initRemoveTriggerZ)
    {
        speed = initSpeed;
        bonusSpawnZ = initSpawnTriggerZ;
        removeTriggerZ = initRemoveTriggerZ;
        isScrolling = true;
    }
    void Start()
    {
        InstanciateBonus();
    }
    private void InstanciateBonus()
    {
        Vector3 initialCoordinate = new(0f, 0f, bonusSpawnZ);

        BarleyInstanciated = Instantiate(BarleyPrefab, initialCoordinate, Quaternion.identity);
        Animator anim = BarleyInstanciated.GetComponentInChildren<Animator>();
        if (anim != null)
            anim.enabled = false;

    }

    private void Update()
    {
        if (HasBonusSpawned)
            UpdateBonus();
    }
    private void UpdateBonus()
    {

        BarleyInstanciated.transform.position += Vector3.back * Time.deltaTime * speed;
        if (BarleyInstanciated.transform.position.z < removeTriggerZ)
        {
            DespawnBonus();
        }
    }
    private void DespawnBonus()
    {
        HasBonusSpawned = false;
        BarleyInstanciated.transform.position = Vector3.forward * bonusSpawnZ;
        BarleyInstanciated.SetActive(true);

        Animator anim = BarleyInstanciated.GetComponentInChildren<Animator>();
        if (anim != null)
            anim.enabled = false;
    }

    public void SpawnBonus()
    {
        if (!HasBonusSpawned)
        {
            List<int> possibleLanes = new() { -14, -7, 0, 7, 14 };
            int laneX = possibleLanes[Random.Range(0, possibleLanes.Count)];
            BarleyInstanciated.transform.position = new Vector3(laneX, BarleyInstanciated.transform.position.y, BarleyInstanciated.transform.position.z);
            HasBonusSpawned = true;
            Animator anim = BarleyInstanciated.GetComponentInChildren<Animator>();
            if (anim != null)
                anim.enabled = true;
        }
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

