using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ScenerySpawner : MonoBehaviour, ISpawner
{

    [SerializeField] private GameObject SpotlightPrefab;
    private readonly List<GameObject> AsleepSpotlightsList = new();
    private readonly List<GameObject> ActiveSpotlightsList = new();
    private readonly int spotlightNumber = 10;
    private float spotlightSpawnZ;
    private float removeTriggerZ;
    private float speed;
    private bool isScrolling = false;

    public void Init(float initSpeed, float initSpawnTriggerZ, float initRemoveTriggerZ)
    {
        spotlightSpawnZ = initSpawnTriggerZ;
        removeTriggerZ = initRemoveTriggerZ;
        speed = initSpeed;
        isScrolling = true;
    }

    private void Start()
    {
        Vector3 initialCoordinate = new(0f, 0f, spotlightSpawnZ);
        for (int i = 0; i < spotlightNumber; i++)
        {
            GameObject newSpotlight = Instantiate(SpotlightPrefab, initialCoordinate, Quaternion.identity);
            newSpotlight.SetActive(false);
            AsleepSpotlightsList.Add(newSpotlight);
        }
    }

    private void Update()
    {
        UpdateSpotlights();
    }

    public void SpawnSpotlight()
    {
        if (AsleepSpotlightsList.Count > 0)
        {
            GameObject newSpotlight = AsleepSpotlightsList[0];
            AsleepSpotlightsList.RemoveAt(0);
            Vector3 newRotation = newSpotlight.transform.eulerAngles;
            Vector3 newPosition = newSpotlight.transform.position;
            if (Random.Range(0, 2) > 0)
            {
                newRotation.y = 0f;
                newPosition.x = -21f;
            }
            else
            {
                newRotation.y = 180f;
                newPosition.x = 21f;
            }
            newSpotlight.transform.eulerAngles = newRotation;
            newSpotlight.transform.position = newPosition;
            newSpotlight.SetActive(true);
            ActiveSpotlightsList.Add(newSpotlight);
        }
    }

    private void UpdateSpotlights()
    {
        for (int i = ActiveSpotlightsList.Count - 1; i >= 0; i--)
        {
            ActiveSpotlightsList[i].transform.position += speed * Time.deltaTime * Vector3.back;

            if (ActiveSpotlightsList[i].transform.position.z < removeTriggerZ)
                DespawnSpotlight(ActiveSpotlightsList[i]);
        }
    }
    private void DespawnSpotlight(GameObject spotlight)
    {
        ActiveSpotlightsList.Remove(spotlight);
        spotlight.transform.position = Vector3.forward * spotlightSpawnZ;
        spotlight.SetActive(false);
        AsleepSpotlightsList.Add(spotlight);
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
