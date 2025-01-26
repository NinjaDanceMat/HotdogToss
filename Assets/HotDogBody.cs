using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class HotDogBody : MonoBehaviour
{
    public Rigidbody2D hotdogBody;
    public float bouncePhysicsMultiplier;
    public float bouncePhysicsUpwards;
    public bool extraHotDog;
    public float minYBonusPos;

    public SpriteRenderer render;
    public Sprite normal;
    public Sprite oof;
    public float oofTimer;

    public AudioSource ouch;


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (Random.Range(0, 4) == 0)
        {

            if (ouch != null)
            {
                ouch.pitch = Random.Range(0.8f, 1.2f);
                ouch.Play();
            }
        }
        if (render !=null)
        {
            render.sprite = oof;
            oofTimer = 0.2f;
        }
        hotdogBody.linearVelocity *= bouncePhysicsMultiplier;
        hotdogBody.linearVelocityY = bouncePhysicsUpwards;
        hotdogBody.linearVelocity = Vector2.ClampMagnitude(hotdogBody.linearVelocity,100);
        if (collision.gameObject.tag == "Wall")
        {
            hotdogBody.linearVelocityX = BubbleGenerator.instance.centerPoint.position.x - transform.position.x;
            hotdogBody.linearVelocityX = Mathf.Max(50, Mathf.Abs(hotdogBody.linearVelocityX)) * (hotdogBody.linearVelocityX >= 0 ? 1 : -1);
        }
        if (collision.gameObject.tag == "Trampoline")
        {
            hotdogBody.linearVelocityY = bouncePhysicsUpwards * 3;
            collision.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        oofTimer -= Time.deltaTime;
        if (render !=null && oofTimer < 0)
        {
            render.sprite = normal;
        }

        if (extraHotDog)
        {
            if (transform.position.y < minYBonusPos)
            {
                HotDogController.instance.extraHotDogs -= 1;
                Destroy(gameObject);
            }
        }
    }
}
