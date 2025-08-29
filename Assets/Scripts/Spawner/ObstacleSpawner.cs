using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private GameObject PinePrefab;
    [SerializeField] private GameObject SnowmanPrefab;
    [SerializeField] private GameObject TinselPrefab;
    [SerializeField] private GameObject LeftChimney;
    [SerializeField] private GameObject RightChimney;
    private ObstacleType LastObstacleType;
    private readonly List<GameObject> AsleepObstaclesList = new();
    private readonly List<GameObject> ActiveObstaclesList = new();
    private readonly float removeTriggerZ = -40f;
    private float obstacleSpawnZ;
    private float speed;
    private bool isScrolling = false;

    private void Start()
    {
        InstanciateObstacles();
        LastObstacleType = ObstacleType.Empty;
    }
    private void InstanciateObstacles()
    {
        Vector3 initialCoordinate = new(0f, 0f, obstacleSpawnZ);
        AsleepObstaclesList.Add(Instantiate(PinePrefab, initialCoordinate, Quaternion.identity));
        AsleepObstaclesList.Add(Instantiate(SnowmanPrefab, initialCoordinate, Quaternion.identity));
        AsleepObstaclesList.Add(Instantiate(TinselPrefab, initialCoordinate, Quaternion.identity));
        AsleepObstaclesList.Add(Instantiate(LeftChimney, initialCoordinate, Quaternion.identity));
        AsleepObstaclesList.Add(Instantiate(RightChimney, initialCoordinate, Quaternion.identity));
        foreach (var obstacle in AsleepObstaclesList)
        {
            Animator anim = obstacle.GetComponentInChildren<Animator>();
            if (anim != null)
                anim.enabled = false;
            obstacle.SetActive(false);
        }
    }

    public void Init(float initSpeed, float initSpawnTriggerZ, float initRemoveTriggerZ)
    {
        speed = initSpeed;
        obstacleSpawnZ = initSpawnTriggerZ;
        isScrolling = true;
    }
    private void Update()
    {
        UpdateObstacles();
    }

    private void UpdateObstacles()
    {
        for (int i = ActiveObstaclesList.Count - 1; i >= 0; i--)
        {
            ActiveObstaclesList[i].transform.position += Vector3.back * Time.deltaTime * speed;
            if (ActiveObstaclesList[i].transform.position.z < removeTriggerZ)
            {
                DespawnObstacle(ActiveObstaclesList[i]);
            }
        }
    }
    public (ObstacleType, int) SpawnRandomObstacle()
    {
        if (AsleepObstaclesList.Count > 0)
        {
            ObstacleType nextObstacleType = ChooseNextObstacleType();
            GameObject nextObstacle = null;
            int xCoords = 0;
            foreach (var obstaclePrefab in AsleepObstaclesList)
            {
                if (obstaclePrefab.GetComponentInChildren<Obstacle>().Type == nextObstacleType)
                {
                    nextObstacle = obstaclePrefab;
                    xCoords = SpawnObstacle(obstaclePrefab);
                    LastObstacleType = nextObstacleType;
                    break;
                }
            }

            if (nextObstacle == null)
                return (ObstacleType.Empty, 0);
            else
                return (nextObstacleType, xCoords);
        }
        else
            return (ObstacleType.Empty, 0);
    }

    private ObstacleType ChooseNextObstacleType()
    {
        List<ObstacleType> availableTypes = new()
        {
            ObstacleType.LeftChimney,
            ObstacleType.RightChimney,
            ObstacleType.Pine,
            ObstacleType.Snowman,
            ObstacleType.Tinsel,
        };
        switch (LastObstacleType)
        {
            case ObstacleType.LeftChimney:
            case ObstacleType.RightChimney:
                availableTypes.Remove(ObstacleType.LeftChimney);
                availableTypes.Remove(ObstacleType.RightChimney);
                availableTypes.Remove(ObstacleType.Tinsel);
                break;
            case ObstacleType.Tinsel:
                availableTypes.Remove(ObstacleType.LeftChimney);
                availableTypes.Remove(ObstacleType.RightChimney);
                availableTypes.Remove(ObstacleType.Tinsel);
                break;
        }
        return availableTypes[Random.Range(0, availableTypes.Count)];
    }

    private int SpawnObstacle(GameObject obstacle)
    {
        int xPos = 0;
        AsleepObstaclesList.Remove(obstacle);
        obstacle.transform.position = Vector3.forward * obstacleSpawnZ;
        obstacle.SetActive(true);
        Animator anim = obstacle.GetComponentInChildren<Animator>();
        if (anim != null)
            anim.enabled = true;
        if (obstacle.GetComponentInChildren<Obstacle>().Type == ObstacleType.Pine)
        {
            Transform childrenTransform = obstacle.transform.Find("Visual");
            List<int> availableLanes = new() { -14, -7, 0, 7, 14 };
            xPos = availableLanes[Random.Range(0, availableLanes.Count)];
            childrenTransform.position = new Vector3(xPos, childrenTransform.position.y, childrenTransform.position.z);
        }
        ActiveObstaclesList.Add(obstacle);
        return xPos;
    }


    private void DespawnObstacle(GameObject obstacle)
    {
        ActiveObstaclesList.Remove(obstacle);
        obstacle.transform.position = Vector3.forward * obstacleSpawnZ;
        Animator anim = obstacle.GetComponentInChildren<Animator>();
        if (anim != null)
            anim.enabled = false;
        obstacle.SetActive(false);
        AsleepObstaclesList.Add(obstacle);
    }

    private void ClearObstacles()
    {
        ResetStateObstacles();
        foreach (var obstacle in AsleepObstaclesList)
        {
            if (obstacle != null)
                Destroy(obstacle);
        }
        AsleepObstaclesList.Clear();
    }

    private void ResetStateObstacles()
    {
        for (int i = ActiveObstaclesList.Count - 1; i >= 0; i--)
        {
            var obstacle = ActiveObstaclesList[i];
            ActiveObstaclesList.RemoveAt(i);
            obstacle.SetActive(false);
            Animator anim = obstacle.GetComponentInChildren<Animator>();
            if (anim != null)
                anim.enabled = false;
            AsleepObstaclesList.Add(obstacle);
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
