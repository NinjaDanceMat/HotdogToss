
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class BubbleGenerator : MonoBehaviour
{
    public GameObject bubblePrefab;
    public GameObject bonusHotDogPrefab;

    public Transform spawnMinPos;
    public Transform spawnMaxPos;

    public float spawnTimer;
    public float spawnTime;

    public float bonusSpawnTimer;
    public float bonusSpawnTime;

    public static BubbleGenerator instance;

    public List<GameObject> bubbles = new List<GameObject>();

    public float minDistanceBetweenBubbles;

    void Awake()
    {
        instance = this;
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
                spawnPrefab = bonusHotDogPrefab;
            }
            bubbles.Add(Instantiate(spawnPrefab, newPos, Quaternion.identity));
        }
    }
}
