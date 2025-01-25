using UnityEngine;

public class BubbleGenerator : MonoBehaviour
{
    public GameObject bubblePrefab;

    public Transform spawnMinPos;
    public Transform spawnMaxPos;

    public float spawnTimer;
    public float spawnTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnTime)
        {
            spawnTimer = 0;
            Instantiate(bubblePrefab,new Vector3(Random.Range(spawnMinPos.position.x, spawnMaxPos.position.x), spawnMinPos.position.y,0),Quaternion.identity);
        }
    }
}
