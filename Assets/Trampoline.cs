using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public static Trampoline instance;

    public Transform startPoint;
    public Transform endPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,endPoint.position,Time.deltaTime*200);
    }
    private void OnEnable()
    {
        transform.position = startPoint.position;
    }
}
