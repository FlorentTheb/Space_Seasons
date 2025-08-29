using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private GameObject CollectablePrefab;
    private readonly List<GameObject> AsleepCollectableList = new();
    private readonly List<GameObject> ActiveCollectableList = new();

    private float collectableSpawnZ;
    private float removeTriggerZ;
    private float CurrentSpeed;
    private bool isScrolling = false;
    private int nbCollectables;
    private float baseSpeed = 40f;
    private float baseDistance = 5f;
    private int nbCollectablesSerie = 7;
    private void Start()
    {
        nbCollectables = 50;
        InstanciateCollectable();

    }
    private void InstanciateCollectable()
    {
        Vector3 initialCoordinate = new(0f, 0f, collectableSpawnZ);
        for (int i = 0; i < nbCollectables; i++)
        {
            AsleepCollectableList.Add(Instantiate(CollectablePrefab, initialCoordinate, Quaternion.identity));
            CollectablePrefab.GetComponentInChildren<Animator>().enabled = false;
        }
    }
    public void Init(float initSpeed, float initSpawnTriggerZ, float initRemoveTriggerZ)
    {
        CurrentSpeed = initSpeed;
        collectableSpawnZ = initSpawnTriggerZ;
        removeTriggerZ = initRemoveTriggerZ;
        isScrolling = true;
    }

    private void Update()
    {
        UpdateCollectables();
    }

    private void UpdateCollectables()
    {
        for (int i = ActiveCollectableList.Count - 1; i >= 0; i--)
        {
            if (Player.Instance.IsMagnetOn && ActiveCollectableList[i].transform.position.z < 30 && ActiveCollectableList[i].transform.position.z > -10)
                ActiveCollectableList[i].GetComponent<Collectable>().isMagnet = true;
            else
            {
                ActiveCollectableList[i].transform.position += CurrentSpeed * Time.deltaTime * Vector3.back;
                if (ActiveCollectableList[i].transform.position.z < removeTriggerZ)
                {
                    ActiveCollectableList[i].GetComponent<Collectable>().isMagnet = false;
                    DespawnCollectable(ActiveCollectableList[i]);
                }
            }
        }
    }

    public void SpawnCollectables(ObstacleType obstacleType, int obstacleXCoordinates)
    {
        if (AsleepCollectableList.Count >= nbCollectablesSerie)
        {
            ECollectableType nextCollectable = ChooseCollectableShape(obstacleType);
            int lane = ChooseCollectableLane(nextCollectable, obstacleType, obstacleXCoordinates);
            SetCollectables(nextCollectable, lane);
        }
    }

    private ECollectableType ChooseCollectableShape(ObstacleType obstacleType)
    {
        List<ECollectableType> availableTypes = new()
        {
            ECollectableType.Straight,
            ECollectableType.Jump,
            ECollectableType.LeftCurve,
            ECollectableType.RightCurve
        };
        switch (obstacleType)
        {
            case ObstacleType.Pine:
                availableTypes.Remove(ECollectableType.Jump);
                break;
            case ObstacleType.Tinsel:
                availableTypes.Remove(ECollectableType.Straight);
                availableTypes.Remove(ECollectableType.LeftCurve);
                availableTypes.Remove(ECollectableType.RightCurve);
                break;
            case ObstacleType.LeftChimney:
            case ObstacleType.RightChimney:
                availableTypes.Remove(ECollectableType.Jump);
                availableTypes.Remove(ECollectableType.LeftCurve);
                availableTypes.Remove(ECollectableType.RightCurve);
                break;
            case ObstacleType.Snowman:
                availableTypes.Remove(ECollectableType.Jump);
                break;
            default:
                break;
        }

        return availableTypes[Random.Range(0, availableTypes.Count)];
    }

    private int ChooseCollectableLane(ECollectableType nextCollectable, ObstacleType obstacleType, int obstacleXCoordinates)
    {
        List<int> possibleLanes = new() { -14, -7, 0, 7, 14 };
        if (obstacleType == ObstacleType.Pine)
        {
            possibleLanes.Remove(obstacleXCoordinates);
        }
        else if (obstacleType == ObstacleType.Snowman)
        {
            possibleLanes.Remove(14);
            possibleLanes.Remove(-14);
        }
        else if (obstacleType == ObstacleType.Tinsel)
        {
            possibleLanes.Remove(14);
            possibleLanes.Remove(-14);
        }
        else if (obstacleType == ObstacleType.LeftChimney)
        {
            possibleLanes.Remove(0);
            possibleLanes.Remove(7);
            possibleLanes.Remove(14);
        }
        else if (obstacleType == ObstacleType.RightChimney)
        {
            possibleLanes.Remove(0);
            possibleLanes.Remove(-7);
            possibleLanes.Remove(-14);
        }

        if (nextCollectable == ECollectableType.LeftCurve)
            possibleLanes.Remove(-14);

        if (nextCollectable == ECollectableType.RightCurve)
            possibleLanes.Remove(14);

        return possibleLanes[Random.Range(0, possibleLanes.Count)];
    }

    private void SetCollectables(ECollectableType type, int lane)
    {
        float distanceBetweenCollectables = baseDistance * (CurrentSpeed / baseSpeed);
        switch (type)
        {
            case ECollectableType.Straight:
                SpawnStraight(lane, distanceBetweenCollectables);
                break;
            case ECollectableType.Jump:
                SpawnJumpCurve(lane, distanceBetweenCollectables);
                break;
            case ECollectableType.RightCurve:
                SpawnRightCurve(lane, distanceBetweenCollectables);
                break;
            case ECollectableType.LeftCurve:
                SpawnLeftCurve(lane, distanceBetweenCollectables);
                break;
            default:
                break;
        }
    }

    private void SpawnStraight(int lane, float distance)
    {
        for (int i = 0; i < nbCollectablesSerie; i++)
        {
            GameObject obj = AsleepCollectableList[0];
            AsleepCollectableList.RemoveAt(0);
            obj.transform.position = new Vector3(lane, 0, (i - (int)(nbCollectablesSerie * .5f)) * distance + collectableSpawnZ);
            obj.GetComponentInChildren<Animator>().enabled = true;
            obj.SetActive(true);
            ActiveCollectableList.Add(obj);
        }
    }

    private void SpawnRightCurve(int lane, float distance)
    {
        float xDistance = 3.5f;
        float maxValue = 7f;

        for (int i = 0; i < nbCollectablesSerie; i++)
        {
            GameObject obj = AsleepCollectableList[0];
            AsleepCollectableList.RemoveAt(0);

            int dist = Mathf.Min(i, nbCollectablesSerie - 1 - i);
            float val = maxValue - dist * xDistance;
            if (val < 0) val = 0;

            obj.transform.position = new Vector3(lane + val, 0, (i - (int)(nbCollectablesSerie * .5f)) * distance + collectableSpawnZ);
            obj.GetComponentInChildren<Animator>().enabled = true;
            obj.SetActive(true);
            ActiveCollectableList.Add(obj);
        }
    }

    private void SpawnLeftCurve(int lane, float distance)
    {
        float xDistance = 3.5f;
        float maxValue = -7f;

        for (int i = 0; i < nbCollectablesSerie; i++)
        {
            GameObject obj = AsleepCollectableList[0];
            AsleepCollectableList.RemoveAt(0);

            int dist = Mathf.Min(i, nbCollectablesSerie - 1 - i);
            float val = maxValue + dist * xDistance;
            if (val > 0) val = 0;

            obj.transform.position = new Vector3(lane + val, 0, (i - (int)(nbCollectablesSerie * .5f)) * distance + collectableSpawnZ);
            obj.GetComponentInChildren<Animator>().enabled = true;
            obj.SetActive(true);
            ActiveCollectableList.Add(obj);
        }
    }


    private void SpawnJumpCurve(int lane, float distance)
    {
        int center = (nbCollectablesSerie - 1) / 2;
        float[] yValues = new float[] { 0f, 6f, 10f, 12f, 10f, 6f, 0f };
        for (int i = 0; i < nbCollectablesSerie; i++)
        {
            GameObject obj = AsleepCollectableList[0];
            AsleepCollectableList.RemoveAt(0);

            obj.transform.position = new Vector3(lane, yValues[i], (i - (int)(nbCollectablesSerie * .5f)) * distance + collectableSpawnZ);
            obj.GetComponentInChildren<Animator>().enabled = true;
            obj.SetActive(true);
            ActiveCollectableList.Add(obj);
        }
    }

    private void DespawnCollectable(GameObject collectable)
    {
        ActiveCollectableList.Remove(collectable);
        collectable.transform.position = Vector3.forward * collectableSpawnZ;
        collectable.SetActive(false);

        collectable.GetComponentInChildren<Animator>().enabled = false;
        AsleepCollectableList.Add(collectable);
    }


    public void UpdateSpeed(float newSpeed)
    {
        CurrentSpeed = newSpeed;
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