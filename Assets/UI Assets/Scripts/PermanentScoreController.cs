using TMPro;
using UnityEngine;

public class PermanentScoreController : MonoBehaviour
{
    public int totalScore;
    public TextMeshProUGUI scoreNumber;
    public static PermanentScoreController instance;

    private void Awake()
    {
        instance = this; 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreNumber.text = totalScore.ToString();
    }
}
