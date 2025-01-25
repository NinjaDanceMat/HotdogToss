using UnityEngine;

public class HotDogBody : MonoBehaviour
{
    public Rigidbody2D hotdogBody;
    public float bouncePhysicsMultiplier;
    public float bouncePhysicsUpwards;
    public bool extraHotDog;
    public float minYBonusPos;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        hotdogBody.linearVelocity *= bouncePhysicsMultiplier;
        hotdogBody.linearVelocityY = bouncePhysicsUpwards;
        hotdogBody.linearVelocity = Vector2.ClampMagnitude(hotdogBody.linearVelocity,100);
    }

    public void Update()
    {
        if (extraHotDog)
        {
            if (transform.position.y < minYBonusPos)
            {
                Destroy(gameObject);
            }
        }
    }
}
