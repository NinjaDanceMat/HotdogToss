using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class HighScoreManager : MonoBehaviour
{
    public List<Score> scores = new List<Score>();
    private string highScoreFilePath;

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

    private void Awake()
    {
        // Set the path to the high score file
        highScoreFilePath = Path.Combine(Application.persistentDataPath, "highscores.json");
        LoadHighScores();
    }

    public void NewScore(int newScore)
    {
        Score newScoreClass = new Score();
        scores.Add(newScoreClass);
        newScoreClass.score = newScore;

        scores.Sort((a, b) => b.score.CompareTo(a.score)); // Sort in descending order

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

        if (!newTopTenScore && !newHighScore)
        {
            SaveHighScores();
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
            highScoreDisplayText.text += score.name + ": " + score.score + "\n";
        }

        SaveHighScores(); // Save to JSON file
    }

    private void SaveHighScores()
    {
        // Convert the list of scores to JSON and save to the file
        HighScoreData data = new HighScoreData { scores = scores };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(highScoreFilePath, json);
    }

    private void LoadHighScores()
    {
        // Check if the file exists
        if (File.Exists(highScoreFilePath))
        {
            // Load the file and deserialize the JSON into the scores list
            string json = File.ReadAllText(highScoreFilePath);
            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
            scores = data?.scores ?? new List<Score>();
        }
        else
        {
            // Create a new file if it doesn't exist
            scores = new List<Score>();
            SaveHighScores();
        }
    }
}

[System.Serializable]
public class Score
{
    public string name;
    public int score;
}

[System.Serializable]
public class HighScoreData
{
    public List<Score> scores;
}
