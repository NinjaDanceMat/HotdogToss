using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public PlayableDirector director;
    public string nextSceneName;
    private bool sceneLoaded = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        director.stopped += OnTimelineStopped;
    }

    private void OnTimelineStopped(PlayableDirector playable)
    {
        if (!sceneLoaded)
        {
            SceneManager.LoadScene(nextSceneName);
            sceneLoaded = true;
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
