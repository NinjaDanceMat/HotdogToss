using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{
    public float maxY;
    public float speed;
    public float minSpeed;
    public float maxSpeed;

    public bool isBonus;

    public GameObject bonusHotDogPrefab;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        BubbleGenerator.instance.bubbles.Remove(gameObject);
        if (isBonus)
        {
            GameObject bonusHotDog = Instantiate(bonusHotDogPrefab, transform.position, Quaternion.identity);
            HotDogBody newBody = bonusHotDog.GetComponent<HotDogBody>();
            newBody.hotdogBody.bodyType = RigidbodyType2D.Dynamic;
            newBody.hotdogBody.linearVelocityY = newBody.bouncePhysicsUpwards;
        }

        Destroy(gameObject);
    }

    public void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    public void Update()
    {
        transform.position = transform.position + new Vector3(0,Time.deltaTime*speed,0);
        if (transform.position.y > maxY)
        {
            BubbleGenerator.instance.bubbles.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
