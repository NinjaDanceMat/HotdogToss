using UnityEngine;

public class WallManager : MonoBehaviour
{

    public GameObject walls;
    public float wallTimer;
    public float wallTime;

    public static WallManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if (wallTimer > 0)
        {
            wallTimer -= Time.deltaTime;
        }
        if (wallTimer < 0)
        {
            walls.SetActive(false);
        }
    }

    // Update is called once per frame
    public void ActivateWalls()
    {
        wallTimer = wallTime;
        walls.SetActive(true);
    }
}
