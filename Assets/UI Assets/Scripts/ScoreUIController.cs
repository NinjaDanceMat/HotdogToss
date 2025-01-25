using UnityEngine;
using TMPro;

public class ScoreUIController : MonoBehaviour
{
    public TextMeshProUGUI lowPointScoreTextBox;
    public int scoreFromHit;
    public Color lowScoreColor = Color.white;
    public Color midScoreColor = Color.yellow;
    public Color highScoreColor = Color.red;
    public Color megaScoreColor = Color.magenta;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //lowPointScoreTextBox.color =  megaScoreColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -50){
                Destroy(gameObject);
            }
        

    }

    public void UpdateScoreValue(){
        lowPointScoreTextBox.text = scoreFromHit.ToString();
        Debug.Log(scoreFromHit);
        if (lowPointScoreTextBox == null)
        {
            Debug.LogError("lowPointScoreTextBox is not assigned!");
            return;
        }

        if (scoreFromHit > 30){
            lowPointScoreTextBox.color = megaScoreColor;
            Debug.Log("Color should have changed for 30+");
        }
        else if (scoreFromHit > 20){
            lowPointScoreTextBox.color = highScoreColor;
            Debug.Log("Color should have changed for 20+");
        }
        else if (scoreFromHit > 10){
            lowPointScoreTextBox.color = midScoreColor;
            Debug.Log("Color should have changed for 10+");
        }
    }
}
