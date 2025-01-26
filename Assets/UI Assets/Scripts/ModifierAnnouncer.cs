using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum PowerUpType{
    doubleDog,
    bunBarrier,
    porkProtector
}

public class ModifierAnnouncer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI announcementText;
    [SerializeField] GameObject announcementPrefab;
    [SerializeField] Color dogColour = new Color();
    [SerializeField] Color bunColour = new Color();
    [SerializeField] Color porkColour = new Color();
    [SerializeField] string doubleDog = "Double Dog!";
    [SerializeField] string bunBarrier = "Bun Barrier!";
    [SerializeField] string porkProtector = "Port Protector!";
    public float torqueRange = 20f;
    public float forceAmount = 20f;
    public ParticleSystem particleSystem;
     
    public static ModifierAnnouncer instance;
    public Transform annoucmentLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(1)){
            GameObject newObject = Instantiate(announcementPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            float torqueAmount = Random.Range(-torqueRange, torqueRange);
            newObject.GetComponent<Rigidbody2D>().AddTorque(torqueAmount);
            announcementText = newObject.GetComponentInChildren<TextMeshProUGUI>();
            particleSystem = newObject.GetComponent<ParticleSystem>();
            whichPowerUp(PowerUpType.bunBarrier);
        }*/
        
    }

    public void AnnoucePowerUp(PowerUpType typeOfPowerup)
    {
        GameObject newObject = Instantiate(announcementPrefab, annoucmentLocation.position, Quaternion.identity);
        float torqueAmount = Random.Range(-torqueRange, torqueRange);
        newObject.GetComponent<Rigidbody2D>().AddTorque(torqueAmount);
        newObject.GetComponent<Rigidbody2D>().AddForce(new Vector2 (0, forceAmount), ForceMode2D.Impulse);
        announcementText = newObject.GetComponentInChildren<TextMeshProUGUI>();
        particleSystem = newObject.GetComponent<ParticleSystem>();
        whichPowerUp(typeOfPowerup);
    }

    public void whichPowerUp(PowerUpType newPowerUp){
        if (newPowerUp == PowerUpType.doubleDog){
            announcementText.text = "Double Dog!";
            announcementText.color = dogColour;
            var main = particleSystem.main;
            main.startColor = new ParticleSystem.MinMaxGradient(dogColour);
            
        }
        else if (newPowerUp == PowerUpType.bunBarrier){
            announcementText.text = "Bun Barrier!";
            announcementText.color = bunColour;
            var main = particleSystem.main;
            main.startColor = new ParticleSystem.MinMaxGradient(bunColour);
        }
        else if (newPowerUp == PowerUpType.porkProtector){
            announcementText.text = "Pork Protector!";
            announcementText.color = porkColour;
            var main = particleSystem.main;
            main.startColor = new ParticleSystem.MinMaxGradient(porkColour);
        }
    }
}
