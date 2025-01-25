using Unity.VisualScripting;
using UnityEngine;

public class ScoreSpawner : MonoBehaviour
{
    [SerializeField] GameObject pointScorePopPrefab;
    public int scoreForThisRun = 0;
    public float torqueAmount;
    public float torqueRange = 50;
    

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            Quaternion spawnRotation = Quaternion.identity;
            GameObject newObject = Instantiate(pointScorePopPrefab, new Vector3(0, 0, 0), spawnRotation);
            ScoreUIController scoreUIController = newObject.GetComponent<ScoreUIController>();
            if (scoreUIController != null){
                scoreUIController.scoreFromHit = scoreForThisRun;
                scoreUIController.UpdateScoreValue();
            }
            torqueAmount = Random.Range(-torqueRange, torqueRange);
            newObject.GetComponent<Rigidbody2D>().AddTorque(torqueAmount);
            scoreForThisRun += 1;
            
            
        }
        
    }
}
