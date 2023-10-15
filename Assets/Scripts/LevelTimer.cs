using UnityEngine;
using UnityEngine.SceneManagement;
using Proyecto26;  

public class LevelTimer : MonoBehaviour
{
    public int targetSceneIndex = 2;
    private float levelStartTime;
    private const string firebaseURL = "https://hue-hustlers-default-rtdb.firebaseio.com/";

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (SceneManager.GetActiveScene().buildIndex == targetSceneIndex)
        {
            levelStartTime = Time.time;
            Debug.Log("Successfullyse");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("enable");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("Successfullydisableo Firebase");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != targetSceneIndex)
        {
            float levelCompletionTime = Time.time;
            SendLevelCompletionTimeToFirebase(levelCompletionTime - levelStartTime);
            Debug.Log("sceneloaded");
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
