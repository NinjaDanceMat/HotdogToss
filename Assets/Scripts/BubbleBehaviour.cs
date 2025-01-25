using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{
    public float maxY;
    public float speed;
    public float minSpeed;
    public float maxSpeed;

    public void OnCollisionEnter2D(Collision2D collision)
    {
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
            Destroy(gameObject);
        }
    }
}
