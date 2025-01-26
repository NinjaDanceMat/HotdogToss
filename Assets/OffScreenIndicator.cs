using UnityEngine;

public class OffScreenIndicator : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public SpriteRenderer arrow; // Reference to the arrow's SpriteRenderer
    public float margin = 50f; // Margin around the screen edge for the arrow

    private Camera mainCamera;
    private Vector2 screenBounds;

    void Start()
    {
        mainCamera = Camera.main;
        screenBounds = new Vector2(Screen.width, Screen.height);
        arrow.enabled = false; // Start with the arrow hidden
    }

    void Update()
    {
        Vector3 playerScreenPosition = mainCamera.WorldToScreenPoint(player.position);

        // Check if the player is off-screen
        if (playerScreenPosition.x < 0 || playerScreenPosition.x > screenBounds.x ||
            playerScreenPosition.y < 0 || playerScreenPosition.y > screenBounds.y)
        {
            arrow.enabled = true;

            // Clamp the arrow position to the screen edge with margin
            Vector3 clampedPosition = playerScreenPosition;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, margin, screenBounds.x - margin);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, margin, screenBounds.y - margin);

            // Convert clamped position back to world coordinates
            Vector3 arrowWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(clampedPosition.x, clampedPosition.y, 10f));
            arrow.transform.position = arrowWorldPosition;

            // Point the arrow towards the player
            Vector2 direction = (player.position - arrow.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle ); // Offset for 2D upward arrow orientation
        }
        else
        {
            arrow.enabled = false; // Hide the arrow when the player is on screen
        }
    }
}
