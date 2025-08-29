using UnityEngine;
using System.Collections.Generic;
public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private GroundSpawner GroundSpawner;
    [SerializeField] private ScenerySpawner ScenerySpawner;
    [SerializeField] private ObstacleSpawner ObstacleSpawner;
    [SerializeField] private CollectableSpawner CollectableSpawner;
    [SerializeField] private BonusSpawner BonusSpawner;
    [SerializeField] private CheckpointSpawner CheckpointSpawner;
    [SerializeField] private Player Player;
    [SerializeField] private float MininmumSpeed = 40f;
    [SerializeField] private float MaximumSpeed = 80f;
    [SerializeField] private float speedIncreasingFactor;
    [SerializeField] private float RemoveTriggerZ = -80f;
    [SerializeField] private float SpawnTriggerZ = 160;
    [SerializeField] private float DistanceBetweenSections = 80f;
    [SerializeField] private float DistanceBetweenSpotlights = 40f;
    [SerializeField] private float DistanceBetweenCheckpoints = 2000f;
    [SerializeField] private float DistanceForObstacle = 40f;
    [SerializeField] private float obstacleSpawnProbability = .6f;
    [SerializeField] private float collectableSpawnProbability = .8f;
    [SerializeField] private float bonusSpawnProbability = .1f;
    private float CurrentSpeed;
    private float totalDistance = 0f;
    private float distanceReached = 0f;

    private float obstacleTimer = 0f;
    private float spotlightTimer = 0f;
    private float checkpointTimer = 0f;

    private void Awake()
    {
        CurrentSpeed = MininmumSpeed;
    }

    private void Start()
    {
        GroundSpawner.Init(CurrentSpeed, SpawnTriggerZ, RemoveTriggerZ);
        ScenerySpawner.Init(CurrentSpeed, SpawnTriggerZ, RemoveTriggerZ);
        ObstacleSpawner.Init(CurrentSpeed, SpawnTriggerZ, RemoveTriggerZ);
        CollectableSpawner.Init(CurrentSpeed, SpawnTriggerZ, RemoveTriggerZ);
        BonusSpawner.Init(CurrentSpeed, SpawnTriggerZ, RemoveTriggerZ);
        CheckpointSpawner.Init(CurrentSpeed, SpawnTriggerZ, RemoveTriggerZ);
        ScoreManager.Instance.UpdateCurrentSpeed(CurrentSpeed);
    }
    private void Update()
    {
        obstacleTimer += Time.deltaTime;
        spotlightTimer += Time.deltaTime;
        checkpointTimer += Time.deltaTime;
        distanceReached += Time.deltaTime * CurrentSpeed;
        totalDistance += Time.deltaTime * CurrentSpeed;
        ObstacleType obstacleType = ObstacleType.Empty;
        if (totalDistance > DistanceBetweenCheckpoints - SpawnTriggerZ)
        {
            CheckpointSpawner.SpawnCheckpoint();
            if (totalDistance > DistanceBetweenCheckpoints)
            {
                CurrentSpeed = MininmumSpeed;
                totalDistance = 0f;
                distanceReached = 0f;
                UpdateSpeed();
            }
        }
        else if (obstacleTimer > (DistanceBetweenSections / CurrentSpeed) && totalDistance < DistanceBetweenCheckpoints - SpawnTriggerZ - 60f)
        {
            obstacleTimer = 0;
            spotlightTimer = 0;
            int xCoordinates = 0;
            float randObstacle = Random.Range(0f, 1f);
            float randBonus = Random.Range(0f, 1f);
            if (randObstacle < obstacleSpawnProbability)
                (obstacleType, xCoordinates) = ObstacleSpawner.SpawnRandomObstacle();
            else if (randBonus < bonusSpawnProbability)
            {
                BonusSpawner.SpawnBonus();
            }

            if (Random.Range(0f, 1f) < collectableSpawnProbability)
                CollectableSpawner.SpawnCollectables(obstacleType, xCoordinates);
        }

        if (spotlightTimer > (DistanceBetweenSpotlights / CurrentSpeed) && obstacleType == ObstacleType.Empty)
        {
            ScenerySpawner.SpawnSpotlight();
            spotlightTimer = 0;
        }
        CheckDistance();
    }

    private void UpdateSpeed()
    {
        GroundSpawner.UpdateSpeed(CurrentSpeed);
        ObstacleSpawner.UpdateSpeed(CurrentSpeed);
        CollectableSpawner.UpdateSpeed(CurrentSpeed);
        BonusSpawner.UpdateSpeed(CurrentSpeed);
        ScenerySpawner.UpdateSpeed(CurrentSpeed);
        CheckpointSpawner.UpdateSpeed(CurrentSpeed);
        ScoreManager.Instance.UpdateCurrentSpeed(CurrentSpeed);
    }

    private void CheckDistance()
    {
        if (distanceReached > 200f)
        {
            CurrentSpeed *= speedIncreasingFactor;
            CurrentSpeed = Mathf.Clamp(CurrentSpeed, MininmumSpeed, MaximumSpeed);
            distanceReached = 0f;
            UpdateSpeed();
        }
    }
}
