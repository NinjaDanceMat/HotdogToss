using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum HotDogState
{ 
    direction,
    velocity,
    bouncing
}


public class HotDogController : MonoBehaviour
{
    public HotDogState currentState = HotDogState.direction;

    public Transform hotdogTransform;

    public float velocityMultiplier;

    public float currentVelocity;
    public float velocityIncreseSpeed;

    public Transform arrow;
    public float maxVelocity;
    public float minArrowScale;
    public float maxArrowScale;

    public Rigidbody2D hotdogBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics2D.gravity *= 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == HotDogState.direction)
        {
            Vector3 mouse_pos = Input.mousePosition;
            Vector3 object_pos = Camera.main.WorldToScreenPoint(hotdogTransform.position);
            mouse_pos.x = mouse_pos.x - object_pos.x;
            mouse_pos.y = mouse_pos.y - object_pos.y;
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            hotdogTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

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

            arrow.localScale = new Vector3(Mathf.Lerp(minArrowScale, maxArrowScale, currentVelocity/maxVelocity), arrow.localScale.y, arrow.localScale.z);

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                currentState = HotDogState.bouncing;
                hotdogBody.bodyType = RigidbodyType2D.Dynamic;
                hotdogBody.linearVelocity = transform.right * currentVelocity * velocityMultiplier;
            }
        }
        if (currentState == HotDogState.bouncing)
        {

        }
    }
}
