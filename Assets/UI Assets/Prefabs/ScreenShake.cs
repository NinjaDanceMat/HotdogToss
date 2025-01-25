using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }

    [Header("Shake Settings")]
    public float shakeDuration = 0.5f; // Default duration of the shake
    public float shakeMagnitude = 0.1f; // Default strength of the shake
    public float dampingSpeed = 1.0f; // Speed at which the shake diminishes

    private float currentShakeDuration;
    private Vector3 initialPosition;
    private bool isShaking = false;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if (isShaking)
        {
            if (currentShakeDuration > 0)
            {
                // Apply shake offset
                Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
                transform.localPosition = initialPosition + shakeOffset;

                // Reduce shake duration
                currentShakeDuration -= Time.deltaTime * dampingSpeed;
            }
            else
            {
                // Reset shake state
                isShaking = false;
                currentShakeDuration = 0f;
                transform.localPosition = initialPosition;
            }
        }
    }

    /// <summary>
    /// Triggers a screen shake with custom duration and magnitude.
    /// </summary>
    /// <param name="duration">Duration of the shake.</param>
    /// <param name="magnitude">Magnitude of the shake.</param>
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        StartShake();
    }

    /// <summary>
    /// Triggers a screen shake using default settings.
    /// </summary>
    public void TriggerShake()
    {
        StartShake();
    }

    private void StartShake()
    {
        isShaking = true;
        currentShakeDuration = shakeDuration;
    }
}
