using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum HotDogState
{ 
    direction,
    velocity,
    bouncing,
    joeover,
    evenMoreJoeover,
    aboluteJoeover
}


public class HotDogController : MonoBehaviour
{
    public HighScoreManager scoreManager;

    public HotDogState currentState = HotDogState.direction;

    public Transform hotdogTosserTransform;
    public Transform hotdogTransform;

    public float velocityMultiplier;

    public float currentVelocity;
    public float velocityIncreseSpeed;

    public Transform arrow;

    public Transform sloMoArrow;
    public Transform sloMoArrowScaler;
    public float maxVelocity;
    public float minArrowScale;
    public float maxArrowScale;

    public Rigidbody2D hotdogBody;
    public float minYPos;

    public bool slowMoModeEnabled;
    public HotDogState slowMoMode;

    public int slowMoModesLeft;
    public int maxSlowMoModes;

    public int lives;
    public int maxLives;
    public TMPro.TextMeshProUGUI livesDisplay;

    public GameObject gameOverScreen;
    public GameObject highscore;
    public TMPro.TextMeshProUGUI finalScore;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        lives = maxLives;
        livesDisplay.text = "Lives: " + lives;
        slowMoModesLeft = maxSlowMoModes;
        Physics2D.gravity *= 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == HotDogState.evenMoreJoeover)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                currentState = HotDogState.aboluteJoeover;
                scoreManager.SaveScore();
            }
        }
        else if (currentState == HotDogState.aboluteJoeover)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                scoreManager.highScoreDisplays.SetActive(false);
                currentState = HotDogState.direction;
                lives = maxLives;
                PermanentScoreController.instance.totalScore = 0;
            }
        }

        if (currentState == HotDogState.direction)
        {

            Vector3 mouse_pos = Input.mousePosition;
            Vector3 object_pos = Camera.main.WorldToScreenPoint(hotdogTosserTransform.position);
            mouse_pos.x = mouse_pos.x - object_pos.x;
            mouse_pos.y = mouse_pos.y - object_pos.y;
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            hotdogTosserTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                currentState = HotDogState.velocity;
                currentVelocity = 0;
            }
        }
        if (currentState == HotDogState.velocity)
        {
            currentVelocity += Time.deltaTime * velocityIncreseSpeed;

            if (currentVelocity > maxVelocity)
            {
                currentVelocity = maxVelocity;
            }

            arrow.localScale = new Vector3(Mathf.Lerp(minArrowScale, maxArrowScale, currentVelocity / maxVelocity), arrow.localScale.y, arrow.localScale.z);

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                currentState = HotDogState.bouncing;
                hotdogBody.bodyType = RigidbodyType2D.Dynamic;
                hotdogBody.linearVelocity = transform.right * currentVelocity * velocityMultiplier;
            }
        }
        if (currentState == HotDogState.bouncing)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && slowMoModesLeft > 0)
            {

                currentVelocity = 0;
                slowMoMode = HotDogState.direction;
                slowMoModeEnabled = true;
                slowMoModesLeft -= 1;
                sloMoArrow.gameObject.SetActive(true);
                Time.timeScale = 0.1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
            }
            if (hotdogTransform.position.y < minYPos)
            {
                currentState = HotDogState.direction;
                ////DEATH
                ///
                lives -= 1;
                livesDisplay.text = "Lives: " + lives;
                if (lives <= 0)
                {
                    currentState = HotDogState.joeover;
                    gameOverScreen.SetActive(true);
                    finalScore.text = "Final Score: " + PermanentScoreController.instance.totalScore;
                }


                ScoreSpawner.instance.scoreForThisRun = 0;
                slowMoModesLeft = maxSlowMoModes;
                slowMoModeEnabled = false;
               
                hotdogBody.linearVelocity = Vector3.zero;
                hotdogBody.bodyType = RigidbodyType2D.Kinematic;
                hotdogTransform.localPosition = Vector3.zero;
                hotdogTransform.localRotation = Quaternion.identity;
                arrow.localScale = new Vector3(minArrowScale, arrow.localScale.y, arrow.localScale.z);
            }
            if (slowMoModeEnabled)
            {
                if (slowMoMode == HotDogState.direction)
                {
                    Vector3 mouse_pos = Input.mousePosition;
                    Vector3 object_pos = Camera.main.WorldToScreenPoint(sloMoArrow.position);
                    mouse_pos.x = mouse_pos.x - object_pos.x;
                    mouse_pos.y = mouse_pos.y - object_pos.y;
                    float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
                    sloMoArrow.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                    currentVelocity += Time.deltaTime * velocityIncreseSpeed * 10;

                    if (currentVelocity > maxVelocity)
                    {
                        currentVelocity = maxVelocity;
                    }
                    sloMoArrowScaler.localScale = new Vector3(Mathf.Lerp(minArrowScale, maxArrowScale, currentVelocity / maxVelocity), sloMoArrowScaler.localScale.y, sloMoArrowScaler.localScale.z);

                    if (Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        slowMoModeEnabled = false;
                        sloMoArrow.gameObject.SetActive(false);
                        hotdogBody.linearVelocity = sloMoArrow.right * currentVelocity * velocityMultiplier;
                        Time.timeScale = 1;
                        Time.fixedDeltaTime = 0.02f * Time.timeScale;
                    }
                }
            }
        }
        if (currentState == HotDogState.joeover)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                currentState = HotDogState.evenMoreJoeover;
                gameOverScreen.SetActive(false);
                scoreManager.NewScore(PermanentScoreController.instance.totalScore);
            }
        }
    }

   
}
