using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum PowerUpType{
    doubleDog,
    bunBarrier,
    portProtector
}

public class ModifierAnnouncer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI announcementText;
    [SerializeField] GameObject announcementPrefab;
    [SerializeField] Color32 dogColour;
    [SerializeField] Color32 bunColour;
    [SerializeField] Color32 porkColour;
    [SerializeField] string doubleDog = "Double Dog!";
    [SerializeField] string bunBarrier = "Bun Barrier!";
    [SerializeField] string porkProtector = "Port Protector!";
    public float torqueRange = 10f;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)){
            GameObject newObject = Instantiate(announcementPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            float torqueAmount = Random.Range(-torqueRange, torqueRange);
            newObject.GetComponent<Rigidbody2D>().AddTorque(torqueAmount);
        }
        
    }

    public void whichPowerUp(PowerUpType newPowerUp){
        if (newPowerUp == PowerUpType.doubleDog){

        }
    }
}
