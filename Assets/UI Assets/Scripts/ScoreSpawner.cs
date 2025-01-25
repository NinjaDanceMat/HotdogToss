using UnityEngine;

public class ScoreSpawner : MonoBehaviour
{
    [SerializeField] GameObject pointScorePopPrefab;
    public int scoreForThisRun = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            Vector3 screenPosition = Input.mousePosition;
            Quaternion spawnRotation = Quaternion.identity;
            GameObject newObject = Instantiate(pointScorePopPrefab, new Vector3(0, 0, 0), spawnRotation);
            ScoreUIController scoreUIController = newObject.GetComponent<ScoreUIController>();
            if (scoreUIController != null){
                scoreUIController.scoreFromHit = scoreForThisRun;
                scoreUIController.UpdateScoreValue();
            }
            scoreForThisRun += 1;


            
        }
    }
}
