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
    public ParticleSystem lines;

    public SpriteRenderer launchArrow;

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

    // Camera zoom variables
    public float zoomInSize = 3f; // Zoomed-in size (for orthographic cameras)
    public float zoomOutSize = 5f; // Default size (for orthographic cameras)
    public float zoomSpeed = 5f; // How fast the zoom effect happens

    private Camera mainCamera;

    public Vector3 defaultCamTransform;

    public GameObject sloMo1;
    public GameObject sloMo2;
    public GameObject sloMo3;

    public GameObject lives1;
    public GameObject lives2;
    public GameObject lives3;

    public float sloMoTimer;
    public float maxSloMoTimer;

    public int extraHotDogs;

    public static HotDogController instance;

    void Start()
    {
        lives = maxLives;

        slowMoModesLeft = maxSlowMoModes;
        UpdateSloMoDisplay();
        UpdateLivesDisplay();
        Physics2D.gravity *= 10;

        mainCamera = Camera.main; // Get the main camera
        defaultCamTransform = mainCamera.transform.position;
    }

    private void Awake()
    {
        instance = this;
    }

    public void UpdateSloMoDisplay()
    {
        sloMo1.SetActive(false);
        sloMo2.SetActive(false);
        sloMo3.SetActive(false);
        if (slowMoModesLeft > 0)
        {
            sloMo1.SetActive(true);
        }
        if (slowMoModesLeft > 1)
        {
            sloMo2.SetActive(true);
        }
        if (slowMoModesLeft > 2)
        {
            sloMo3.SetActive(true);
        }
    }

    public void UpdateLivesDisplay()
    {
        lives1.SetActive(false);
        lives2.SetActive(false);
        lives3.SetActive(false);
        if (lives > 0)
        {
            lives1.SetActive(true);
        }
        if (lives > 1)
        {
            lives2.SetActive(true);
        }
        if (lives > 2)
        {
            lives3.SetActive(true);
        }
    }

    void Update()
    {
        if (slowMoModeEnabled)
        {
            sloMoTimer += Time.deltaTime*10;
            ScreenShake.Instance.TriggerShake();
        }
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
                UpdateLivesDisplay();
                PermanentScoreController.instance.totalScore = 0;
            }
        }

        else if (currentState == HotDogState.direction)
        {
            Trampoline.instance.gameObject.SetActive(false);
            WallManager.instance.Deactive();

            Vector3 mouse_pos = Input.mousePosition;
            Vector3 object_pos = Camera.main.WorldToScreenPoint(hotdogTosserTransform.position);
            mouse_pos.x -= object_pos.x;
            mouse_pos.y -= object_pos.y;
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            hotdogTosserTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                currentState = HotDogState.velocity;
                currentVelocity = 0;
            }
        }
        else if (currentState == HotDogState.velocity)
        {
            currentVelocity += Time.deltaTime * velocityIncreseSpeed;

            if (currentVelocity > maxVelocity)
            {
                currentVelocity = maxVelocity;
            }

            arrow.localScale = new Vector3(Mathf.Lerp(minArrowScale, maxArrowScale, currentVelocity / maxVelocity), arrow.localScale.y, arrow.localScale.z);

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                launchArrow.enabled = false; 
                currentState = HotDogState.bouncing;
                hotdogBody.bodyType = RigidbodyType2D.Dynamic;
                hotdogBody.linearVelocity = transform.right * currentVelocity * velocityMultiplier;
            }
        }
        else if (currentState == HotDogState.bouncing)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && slowMoModesLeft > 0)
            {
                currentVelocity = 0;
                slowMoMode = HotDogState.direction;
                sloMoTimer = 0;
                slowMoModeEnabled = true;
                lines.Play();
                slowMoModesLeft -= 1;
                UpdateSloMoDisplay();
                sloMoArrow.gameObject.SetActive(true);
                Time.timeScale = 0.1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;

                // Start zooming in
                StartCoroutine(LerpCameraZoom(zoomInSize,hotdogBody.transform.position));
            }

            if (hotdogTransform.position.y < minYPos && extraHotDogs <= 0)
            {
                ResetAfterBounce();
            }

            if (slowMoModeEnabled)
            {
                if (slowMoMode == HotDogState.direction)
                {
                    Vector3 mouse_pos = Input.mousePosition;
                    Vector3 object_pos = Camera.main.WorldToScreenPoint(sloMoArrow.position);
                    mouse_pos.x -= object_pos.x;
                    mouse_pos.y -= object_pos.y;
                    float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
                    sloMoArrow.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                    currentVelocity += Time.deltaTime * velocityIncreseSpeed * 10;

                    if (currentVelocity > maxVelocity)
                    {
                        currentVelocity = maxVelocity;
                    }
                    sloMoArrowScaler.localScale = new Vector3(Mathf.Lerp(minArrowScale, maxArrowScale, currentVelocity / maxVelocity), sloMoArrowScaler.localScale.y, sloMoArrowScaler.localScale.z);

                    if (Input.GetKeyUp(KeyCode.Mouse0) || sloMoTimer > maxSloMoTimer)
                    {
                        lines.Stop();
                        slowMoModeEnabled = false;
                        sloMoArrow.gameObject.SetActive(false);
                        hotdogBody.linearVelocity = sloMoArrow.right * currentVelocity * velocityMultiplier;
                        Time.timeScale = 1;
                        Time.fixedDeltaTime = 0.02f * Time.timeScale;

                        // Start zooming out
                        StartCoroutine(LerpCameraZoom(zoomOutSize, defaultCamTransform));
                    }
                }
            }
        }
        else if (currentState == HotDogState.joeover)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                currentState = HotDogState.evenMoreJoeover;
                gameOverScreen.SetActive(false);
                scoreManager.NewScore(PermanentScoreController.instance.totalScore);
            }
        }
    }

    private void ResetAfterBounce()
    {
        launchArrow.enabled = true;
        Trampoline.instance.gameObject.SetActive(false);
        WallManager.instance.Deactive();
        currentState = HotDogState.direction;
        lives -= 1;
        UpdateLivesDisplay();
       

        if (lives <= 0)
        {
            currentState = HotDogState.joeover;
            gameOverScreen.SetActive(true);
            finalScore.text = "Final Score: " + PermanentScoreController.instance.totalScore;
        }

        ScoreSpawner.instance.scoreForThisRun = 0;
        slowMoModesLeft = maxSlowMoModes;
        lines.Stop();
        UpdateSloMoDisplay();
        slowMoModeEnabled = false;
        sloMoArrow.gameObject.SetActive(false);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        hotdogBody.linearVelocity = Vector3.zero;
        hotdogBody.bodyType = RigidbodyType2D.Kinematic;
        hotdogTransform.localPosition = Vector3.zero;
        hotdogTransform.localRotation = Quaternion.identity;
        arrow.localScale = new Vector3(minArrowScale, arrow.localScale.y, arrow.localScale.z);

        // Ensure the camera zooms out when the game resets
        StartCoroutine(LerpCameraZoom(zoomOutSize, defaultCamTransform));
    }

    private System.Collections.IEnumerator LerpCameraZoom(float targetSize, Vector3 targetPos)
    {
        float startSize = mainCamera.orthographicSize;
        Vector3 startPosition = mainCamera.transform.position;
        Vector3 targetPosition = new Vector3(targetPos.x, targetPos.y, mainCamera.transform.position.z);
        targetPosition = Vector3.Lerp(defaultCamTransform,targetPosition,0.1f);

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.unscaledDeltaTime * zoomSpeed;

            // Smoothly interpolate the camera size and position
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime);
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);

            yield return null;
        }

        // Ensure final values are set
        mainCamera.orthographicSize = targetSize;
        mainCamera.transform.position = targetPosition;
    }
}

