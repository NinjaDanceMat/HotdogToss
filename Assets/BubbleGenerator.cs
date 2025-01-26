
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class BubbleGenerator : MonoBehaviour
{
    public GameObject bubblePrefab;
    public GameObject bonusHotDogPrefab;
    public GameObject wallPrefab;
    public GameObject trampolinePrefab;

    public Transform spawnMinPos;
    public Transform spawnMaxPos;

    public float spawnTimer;
    public float spawnTime;

    public float bonusSpawnTimer;
    public float bonusSpawnTime;

    public static BubbleGenerator instance;

    public List<GameObject> bubbles = new List<GameObject>();

    public float minDistanceBetweenBubbles;


    public Transform centerPoint;

    private Dictionary<int, float> weights = new Dictionary<int, float>();
    private float totalWeight;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // Initialize weights (equal probabilities)
        weights[0] = 1.0f;
        weights[1] = 1.0f;
        weights[2] = 1.0f;
        totalWeight = weights[0] + weights[1] + weights[2];
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        bonusSpawnTimer += Time.deltaTime;
        if (spawnTimer > spawnTime)
        {
            spawnTimer = 0;

            bool foundSpawn = false;
            float trySpawnCount = 0;
            Vector3 newPos = Vector3.zero;
            while (!foundSpawn && trySpawnCount < 10)
            {
                trySpawnCount++;
                newPos = new Vector3(Random.Range(spawnMinPos.position.x, spawnMaxPos.position.x), spawnMinPos.position.y, 0);
                foundSpawn = true;
                foreach (GameObject bubble in bubbles)
                {
                    if (Vector3.Distance(bubble.transform.position,newPos) < minDistanceBetweenBubbles)
                    {
                        foundSpawn = false;
                    }
                }
                    
            }
            GameObject spawnPrefab = bubblePrefab;
            if (bonusSpawnTimer > bonusSpawnTime)
            {
                bonusSpawnTimer = 0;
                spawnPrefab = GetSpawnPrefab();
  

            }
            bubbles.Add(Instantiate(spawnPrefab, newPos, Quaternion.identity));
        }


    }
    public GameObject GetSpawnPrefab()
    {
        // Get a random value between 0 and totalWeight
        float randomValue = Random.Range(0, totalWeight);

        // Find which option corresponds to the random value
        int chosenOption = -1;
        float cumulativeWeight = 0;
        foreach (var pair in weights)
        {
            cumulativeWeight += pair.Value;
            if (randomValue < cumulativeWeight)
            {
                chosenOption = pair.Key;
                break;
            }
        }

        // Adjust weights to make the chosen option less likely
        if (chosenOption != -1)
        {
            weights[chosenOption] *= 0.5f; // Reduce the weight of the chosen option
            NormalizeWeights(); // Normalize weights to keep them meaningful
        }

        // Return the appropriate prefab
        switch (chosenOption)
        {
            case 0: return bonusHotDogPrefab;
            case 1: return wallPrefab;
            case 2: return trampolinePrefab;
            default: return null;
        }
    }

    private void NormalizeWeights()
    {
        totalWeight = 0;
        foreach (var key in weights.Keys)
        {
            totalWeight += weights[key];
        }
    }
}
