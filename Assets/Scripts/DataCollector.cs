using System;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.SceneManagement;

public class DataCollector : MonoBehaviour
{
    private const string firebaseURL = "https://hue-hustlers-default-rtdb.firebaseio.com/";
    private int[] colorSwitchCounts = new int[3] { 0, 0, 0 };

    private int[] colorSwitchCounts = new int[3] { 0, 0, 0 };
    private string currentLevel => SceneManager.GetActiveScene().name;
    private string playthroughId;

    // List of scenes to track
    public List<int> targetSceneIndices = new List<int>();
    private float levelStartTime;
    private bool isLevelStarted = false;
    private int previousSceneIndex = -1;
    private string previousSceneName;

    void Awake()
    {
        ServiceLocator.DataCollector = this;
        if (s_instance != null && s_instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void GeneratePlaythroughId()
    {
        playthroughId = Guid.NewGuid().ToString();
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (targetSceneIndices.Contains(SceneManager.GetActiveScene().buildIndex))
        {
            levelStartTime = Time.time;
            isLevelStarted = true;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (targetSceneIndices.Contains(previousSceneIndex) && !targetSceneIndices.Contains(scene.buildIndex) && isLevelStarted)
        {
            float levelCompletionTime = Time.time - levelStartTime;
            SendLevelCompletionTimeToFirebase(levelCompletionTime);
            isLevelStarted = false;
        }

        if (targetSceneIndices.Contains(scene.buildIndex))
        {
            levelStartTime = Time.time;
            isLevelStarted = true;
        }

        previousSceneName = scene.name;
        previousSceneIndex = scene.buildIndex;
    }

    public void CollectColorSwitch(LevelColor color)
    {
        colorSwitchCounts[(int)color]++;
    }

    public int[] GetColorSwitchCount()
    {
        return colorSwitchCounts;
    }

    public void SendColorSwitchCountsToFirebase()
    {
        ColorSwitchCountsData colorData = new ColorSwitchCountsData
        {
            Red = colorSwitchCounts[(int)LevelColor.Red],
            Blue = colorSwitchCounts[(int)LevelColor.Blue],
            Yellow = colorSwitchCounts[(int)LevelColor.Yellow]
        };
        string colorJsonData = JsonUtility.ToJson(colorData);

        RestClient.Post(firebaseURL + "playthroughs/" + currentLevel + "/colorSwitchCounts.json", colorJsonData)
            .Then(response =>
            {
                Debug.Log("Successfully sent color switch counts to Firebase for " + currentLevel);
            })
            .Catch(error =>
            {
                Debug.LogError("Error sending color switch counts to Firebase: " + error.Message);
            });
    }


    public void ResetColorSwitchCounts()
    {
        for (int i = 0; i < colorSwitchCounts.Length; i++)
        {
            colorSwitchCounts[i] = 0;
        }
    }

    private void SendLevelCompletionTimeToFirebase(float timeTaken)
    {
        // Endpoint to get the completion times for the current level
        string completionTimeEndpoint = firebaseURL + "playthroughs/" + previousSceneName + "/completionTime.json";

        RestClient.Get(completionTimeEndpoint).Then(response =>
        {
            bool alreadyExists = false;

            if (!string.IsNullOrEmpty(response.Text) && response.Text != "null")
            {
                // Convert the JSON response to a dictionary of completion times
                Dictionary<string, LevelCompletionData> existingTimes;
                try
                {
                    existingTimes = JsonUtility.FromJson<Dictionary<string, LevelCompletionData>>(response.Text);
                }
                catch
                {
                    existingTimes = null;
                }

                // Check if the new time is already in the list of existing times
                if (existingTimes != null)
                {
                    foreach (var entry in existingTimes)
                    {
                        if (Mathf.Approximately(entry.Value.CompletionTime, timeTaken))
                        {
                            alreadyExists = true;
                            break;
                        }
                    }
                }
            }

            // If the completion time is not already present, then add it
            if (!alreadyExists)
            {
                LevelCompletionData levelData = new LevelCompletionData
                {
                    CompletionTime = timeTaken
                };
                string levelJsonData = JsonUtility.ToJson(levelData);

                // Now we're using a Post request to the specific level's endpoint
                RestClient.Post(completionTimeEndpoint, levelJsonData)
                    .Then(postResponse =>
                    {
                        Debug.Log("Successfully sent level completion time to Firebase for " + previousSceneName);
                    })
                    .Catch(postError =>
                    {
                        Debug.LogError("Error sending level completion time to Firebase: " + postError.Message);
                    });
            }
            else
            {
                Debug.Log("Completion time for " + previousSceneName + " already exists. Not sending again.");
            }

        }).Catch(error =>
        {
            Debug.LogError("Error fetching existing completion times from Firebase: " + error.Message);
        });
    }


    [System.Serializable]
    public class ColorSwitchCountsData
    {
        public int Red;
        public int Blue;
        public int Yellow;
    }

    [System.Serializable]
    private class LevelCompletionData
    {
        public float CompletionTime;
    }
}
