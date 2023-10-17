using UnityEngine;
using UnityEngine.SceneManagement;
using Proyecto26;
using System.Collections.Generic; // Make sure this is added

public class LevelTimer : MonoBehaviour
{
    public int targetSceneIndex = 2;
    private float levelStartTime;
    private const string firebaseURL = "https://hue-hustlers-default-rtdb.firebaseio.com/";

    // List of scenes to ignore
    public List<int> ignoredSceneIndices = new List<int> { 0 };

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (SceneManager.GetActiveScene().buildIndex == targetSceneIndex)
        {
            levelStartTime = Time.time;
            Debug.Log("Timer started");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("OnEnable called");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("OnDisable called");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!ignoredSceneIndices.Contains(scene.buildIndex) && scene.buildIndex != targetSceneIndex)
        {
            float levelCompletionTime = Time.time - levelStartTime;
            SendLevelCompletionTimeToFirebase(levelCompletionTime);
            Debug.Log("Scene loaded and data sent to Firebase");
        }
        if (scene.buildIndex == targetSceneIndex)
        {
            levelStartTime = Time.time;
        }
    }

    private void SendLevelCompletionTimeToFirebase(float timeTaken)
    {
        LevelCompletionData data = new LevelCompletionData
        {
            CompletionTime = timeTaken
        };
        string jsonData = JsonUtility.ToJson(data);

        RestClient.Post(firebaseURL + "levelCompletionTime.json", jsonData).Then(response =>
        {
            Debug.Log("Successfully sent level completion time to Firebase");
        }).Catch(error =>
        {
            Debug.LogError("Error sending level completion time to Firebase: " + error.Message);
        });
    }

    [System.Serializable]
    private class LevelCompletionData
    {
        public float CompletionTime;
    }
}
