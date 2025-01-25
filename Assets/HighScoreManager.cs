
using UnityEngine;
using System.Collections.Generic;

public class HighScoreManager : MonoBehaviour
{
    public List<Score> scores = new List<Score>();

    public GameObject InputName;
    public GameObject newHighScoreOb;
    public GameObject newTopTenScoreOb;
    public GameObject noNewScore;

    public GameObject highScoreDisplays;
    public TMPro.TextMeshProUGUI inputName;

    public TMPro.TextMeshProUGUI highScoreDisplayText;

    public Score mostRecentScore;

    public int maxScores;

    public HotDogController controller;
    public void NewScore(int newScore)
    {
        Score newScoreClass = new Score();
        scores.Add(newScoreClass);
        newScoreClass.score = newScore;

        scores.Sort((a, b) => a.score.CompareTo(b.score));
        scores.Reverse();

        bool newTopTenScore = false;
        bool newHighScore = false;
        if (scores.Count <= maxScores)
        {
            newTopTenScore = true;
        }
        else
        {
            scores.RemoveAt(scores.Count - 1);
            if (scores.Contains(newScoreClass))
            {
                newTopTenScore = true;
            }
        }
        if (scores[0] == newScoreClass)
        {
            newHighScore = true;
        }
        if (newHighScore || newTopTenScore)
        {
            InputName.SetActive(true);
        }
        if (newHighScore)
        {
            newHighScoreOb.SetActive(true);
            mostRecentScore = newScoreClass;
        }
        else if (newTopTenScore)
        {
            newTopTenScoreOb.SetActive(true);
            mostRecentScore = newScoreClass;
        }
        newHighScoreOb.SetActive(newHighScore);
        //noNewScore.SetActive(!newTopTenScore && !newHighScore);
        if (!newTopTenScore && !newHighScore)
        {
            SaveScore();
            controller.currentState = HotDogState.aboluteJoeover;
        }
    }

    public void SaveScore()
    {
        if (mostRecentScore != null)
        {
            mostRecentScore.name = inputName.text;
        }
        InputName.SetActive(false);
        newHighScoreOb.SetActive(false);
        newTopTenScoreOb.SetActive(false);
        highScoreDisplays.SetActive(true);
        highScoreDisplayText.text = "";
        foreach (Score score in scores)
        {
            highScoreDisplayText.text += score.name;
            highScoreDisplayText.text += ": ";
            highScoreDisplayText.text += score.score;
            highScoreDisplayText.text += "\n";
        }

    }
}

public class Score
{
    public string name;
    public int score;
}
