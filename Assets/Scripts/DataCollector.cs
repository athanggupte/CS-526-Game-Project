using System;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.SceneManagement;
using System.Linq;

public class DataCollector : MonoBehaviour
{
    public static int[] targetSceneIndices = new int[] { 2, 3, 4, 5, 6, 7 };
    //Zone controller
    public Dictionary<string, float> zoneTimes = new Dictionary<string, float>();


    private const string firebaseURL = "https://hue-hustlers-default-rtdb.firebaseio.com/";
    private int[] colorSwitchCounts = new int[3] { 0, 0, 0 };
    private int switchCount = 0;
    private string currentLevel => SceneManager.GetActiveScene().name;
    private string playthroughId;
    private int collectedBombCount = 0;
    private int starCount = 0;
    private int bombsDetonatedCount = 0;
    private int gunsCollectedCount = 0;
    private Dictionary<int, bool> bombEnemyDetonatedStatus = new Dictionary<int, bool>();
    private Dictionary<string, int> switchCountPerZone = new Dictionary<string, int>();
    private Dictionary<string, int> collectedBombCountPerZone = new Dictionary<string, int>();
    private Dictionary<string, int> bombsDetonatedCountPerZone = new Dictionary<string, int>();
    private Dictionary<string, int> gunsCollectedCountPerZone = new Dictionary<string, int>();

    // List of scenes to track
    private float levelStartTime;
    private bool isLevelStarted = false;
    private int previousSceneIndex = -1;
    private string previousSceneName;
    private int upKeyClickCount = 0;
    private int spacebarClickCount = 0;
    private int hudSceneIndex = 1;

    void Awake()
    {
        DontDestroyOnLoad(this);
        ServiceLocator.DataCollector = this;
        LevelEvents.LevelEventsInitialized.AddListener(ListenToLevelEvents);
        InitializeBombEnemyDetonatedStatus();
    }

    private void InitializeBombEnemyDetonatedStatus()
    {
        BombBuddy[] bombBuddies = FindObjectsOfType<BombBuddy>();
        foreach (var bombBuddy in bombBuddies)
        {
            bombEnemyDetonatedStatus[bombBuddy.bombID] = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            upKeyClickCount++;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spacebarClickCount++;
        }
    }

    private void ListenToLevelEvents()
    {
        LevelEvents.Instance.OrbColorSwitch.AddListener(CollectColorSwitch);
        LevelEvents.Instance.LevelEnd.AddListener(SendCompleteDataToFirebase);
        LevelEvents.Instance.BombCollect.AddListener(CollectBomb);
        LevelEvents.Instance.StarCollect.AddListener(CollectStar);
        LevelEvents.Instance.ColorBombDetonate.AddListener(ColorBomb);
        LevelEvents.Instance.BombEnemyDetonate.AddListener(BombEnemy);
        LevelEvents.Instance.GunCollect.AddListener(CollectGun);
    }

    private void SendCompleteDataToFirebase()
    {
        if (!Debug.isDebugBuild)
        {
            SendColorSwitchCountsToFirebase();
            SendSwitchCountToFirebase();
            SendBombsCollectedCount();
            SendStarCount();
            SendBombsDetonatedCount();
            SendBombEnemyDetonatedStatus();
            SendKeySpacebarCount();
            SendGunsCollectedCount();
            SendZoneTimesToFirebase();
            SendZoneBombsDetonatedCount();
            SendZoneCollectedBombCount();
            SendZoneCollectedGunsCount();
            SendZoneSwitchCount();
            ResetCounts();
        }
    }

    private void GeneratePlaythroughId()
    {
        playthroughId = Guid.NewGuid().ToString();
    }

    private void Start()
    {
        if (targetSceneIndices.Contains(SceneManager.GetActiveScene().buildIndex))
        {
            levelStartTime = Time.time;
            isLevelStarted = true;
            RecordNewPlayerSession();
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
        // Check if the loaded scene is the HUD scene by index
        if (scene.buildIndex == hudSceneIndex)
        {
            // If it's the HUD scene, do not perform any level completion calculations
            return;
        }

        // Check if we're transitioning from a level to a non-level (and vice versa)
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

        // Update previous scene information
        previousSceneName = scene.name;
        previousSceneIndex = scene.buildIndex;

        // Existing logic for handling this object in new scenes
        if (ServiceLocator.DataCollector != this)
            Destroy(this.gameObject);
    }


    public void CollectColorSwitch(LevelColor color, string zoneName)
    {
        colorSwitchCounts[(int)color]++;
        switchCount++;
        if (zoneName != "")
        {
            if (switchCountPerZone.ContainsKey(zoneName))
            {
                switchCountPerZone[zoneName]++;
            }
            else
            {
                switchCountPerZone[zoneName] = 1;
            }
        }
    }

    public int[] GetColorSwitchCount()
    {
        return colorSwitchCounts;
    }

    public int GetSwitchCount()
    {
        return switchCount;
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

    public void SendSwitchCountToFirebase()
    {
        SwitchCountJsonData switchCountData = new SwitchCountJsonData { SwitchCount = switchCount };
        string switchCountJsonData = JsonUtility.ToJson(switchCountData);
        RestClient.Post(firebaseURL + "playthroughs/" + currentLevel + "/switchCounts.json", switchCountJsonData)
            .Then(response =>
            {
                Debug.Log("Successfully sent switch count to Firebase for " + currentLevel);
            })
            .Catch(error =>
            {
                Debug.LogError("Error sending switch count to Firebase: " + error.Message);
            });
    }

    public void ResetCounts()
    {
        switchCount = 0;
        starCount = 0;
        collectedBombCount = 0;
        bombsDetonatedCount = 0;
        bombEnemyDetonatedStatus = new Dictionary<int, bool>();
        switchCountPerZone = new Dictionary<string, int>();
        collectedBombCountPerZone = new Dictionary<string, int>();
        bombsDetonatedCountPerZone = new Dictionary<string, int>();
        gunsCollectedCountPerZone = new Dictionary<string, int>();
        upKeyClickCount = 0;
        spacebarClickCount = 0;
        gunsCollectedCount = 0;
        for (int i = 0; i < colorSwitchCounts.Length; i++)
        {
            colorSwitchCounts[i] = 0;
        }
}

    public void SendLevelCompletionTimeToFirebase(float timeTaken)
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

    public void UpdateZoneTime(string zoneName, float time)
    {
        if (zoneTimes.ContainsKey(zoneName))
        {
            zoneTimes[zoneName] += time;
        }
        else
        {
            zoneTimes[zoneName] = time;
        }
    }

    public void SendZoneTimesToFirebase()
    {
        foreach (var zone in zoneTimes)
        {
            string zoneEndpoint = firebaseURL + "zonetimes/" + currentLevel + "/" + zone.Key + "/zoneTime.json";
            string zoneJsonData = JsonUtility.ToJson(new ZoneTimeData { ZoneTime = zone.Value });

            RestClient.Post(zoneEndpoint, zoneJsonData)
                .Then(response =>
                {
                    Debug.Log("Successfully sent zone time to Firebase for " + currentLevel + " in " + zone.Key);
                })
                .Catch(error =>
                {
                    Debug.LogError("Error sending zone time to Firebase: " + error.Message);
                });
        }
    }

    public void SendZoneSwitchCount()
    {
        foreach (var zone in switchCountPerZone)
        {
            string zoneEndpoint = firebaseURL + "zonetimes/" + currentLevel + "/" + zone.Key + "/switchCount.json";
            string zoneJsonData = JsonUtility.ToJson(new SwitchCountJsonData { SwitchCount = zone.Value });

            RestClient.Post(zoneEndpoint, zoneJsonData)
                .Then(response =>
                {
                    Debug.Log("Successfully sent zone switch count to Firebase for " + currentLevel + " in " + zone.Key);
                })
                .Catch(error =>
                {
                    Debug.LogError("Error sending zone switch count to Firebase: " + error.Message);
                });
        }
    }

    public void SendZoneBombsDetonatedCount()
    {
        foreach (var zone in bombsDetonatedCountPerZone)
        {
            string zoneEndpoint = firebaseURL + "zonetimes/" + currentLevel + "/" + zone.Key + "/bombsDetonatedCount.json";
            string zoneJsonData = JsonUtility.ToJson(new BombsDetonatedCountJsonData { BombsDetonatedCount = zone.Value });

            RestClient.Post(zoneEndpoint, zoneJsonData)
                .Then(response =>
                {
                    Debug.Log("Successfully sent zone bombs detonated count to Firebase for " + currentLevel + " in " + zone.Key);
                })
                .Catch(error =>
                {
                    Debug.LogError("Error sending zone bombs detonated count to Firebase: " + error.Message);
                });
        }
    }

    public void SendZoneCollectedBombCount()
    {
        foreach (var zone in collectedBombCountPerZone)
        {
            string zoneEndpoint = firebaseURL + "zonetimes/" + currentLevel + "/" + zone.Key + "/collectedBombCount.json";
            string zoneJsonData = JsonUtility.ToJson(new CollectedBombCountJsonData { BombsCollectedCount = zone.Value });

            RestClient.Post(zoneEndpoint, zoneJsonData)
                .Then(response =>
                {
                    Debug.Log("Successfully sent zone collected bomb count to Firebase for " + currentLevel + " in " + zone.Key);
                })
                .Catch(error =>
                {
                    Debug.LogError("Error sending zone collected bomb count to Firebase: " + error.Message);
                });
        }
    }

    public void SendZoneCollectedGunsCount()
    {
        foreach (var zone in gunsCollectedCountPerZone)
        {
            string zoneEndpoint = firebaseURL + "zonetimes/" + currentLevel + "/" + zone.Key + "/gunsCollectedCount.json";
            string zoneJsonData = JsonUtility.ToJson(new CollectedGunCountJsonData { GunsCollectedCount = zone.Value });

            RestClient.Post(zoneEndpoint, zoneJsonData)
                .Then(response =>
                {
                    Debug.Log("Successfully sent zone guns collected count to Firebase for " + currentLevel + " in " + zone.Key);
                })
                .Catch(error =>
                {
                    Debug.LogError("Error sending zone guns collected count to Firebase: " + error.Message);
                });
        }
    }

    public void CollectBomb(LevelColor color, string zoneName)
    {
        collectedBombCount++;
        if(zoneName != "")
        {
            if (collectedBombCountPerZone.ContainsKey(zoneName))
            {
                collectedBombCountPerZone[zoneName] += 1;
            }
            else
            {
                collectedBombCountPerZone[zoneName] = 1;
            }
        }
        
    }

    public void CollectGun(string zoneName)
    {
        gunsCollectedCount++;
        if (zoneName != "")
        {
            if (gunsCollectedCountPerZone.ContainsKey(zoneName))
            {
                gunsCollectedCountPerZone[zoneName] += 1;
            }
            else
            {
                gunsCollectedCountPerZone[zoneName] = 1;
            }
        }
    }

    public void SendBombsCollectedCount()
    {
        CollectedBombCountJsonData collectedBombData = new CollectedBombCountJsonData { BombsCollectedCount = collectedBombCount };
        string collectedBombJsonData = JsonUtility.ToJson(collectedBombData);
        RestClient.Post(firebaseURL + "playthroughs/" + currentLevel + "/bombsCollectedCount.json", collectedBombJsonData)
            .Then(response =>
            {
                Debug.Log("Successfully sent bombs collected count to Firebase for " + currentLevel);
            })
            .Catch(error =>
            {
                Debug.LogError("Error sending bombs collected count to Firebase: " + error.Message);
            });
    }

    public void CollectStar()
    {
        starCount++;
    }

    public void SendStarCount()
    {
        StarCountJsonData starCountData = new StarCountJsonData { StarCount = starCount };
        string starCountJsonData = JsonUtility.ToJson(starCountData);
        RestClient.Post(firebaseURL + "playthroughs/" + currentLevel + "/starCount.json", starCountJsonData)
            .Then(response =>
            {
                Debug.Log("Successfully sent star count to Firebase for " + currentLevel);
            })
            .Catch(error =>
            {
                Debug.LogError("Error sending star count to Firebase: " + error.Message);
            });
    }

    void ColorBomb(LevelColor targetColor, Vector3 position, float radius, string zoneName)
    {
        bombsDetonatedCount++;
        if (zoneName != "")
        {
            if (bombsDetonatedCountPerZone.ContainsKey(zoneName))
            {
                bombsDetonatedCountPerZone[zoneName] += 1;
            }
            else
            {
                bombsDetonatedCountPerZone[zoneName] = 1;
            }
        }

    }

    public void SendBombsDetonatedCount()
    {
        BombsDetonatedCountJsonData bombsDetonatedData = new BombsDetonatedCountJsonData { BombsDetonatedCount = bombsDetonatedCount };
        string bombsDetonatedJsonData = JsonUtility.ToJson(bombsDetonatedData);
        RestClient.Post(firebaseURL + "playthroughs/" + currentLevel + "/bombsDetonatedCount.json", bombsDetonatedJsonData)
            .Then(response =>
            {
                Debug.Log("Successfully sent bombs detonated count to Firebase for " + currentLevel);
            })
            .Catch(error =>
            {
                Debug.LogError("Error sending bombs detonated count to Firebase: " + error.Message);
            });
    }

    public void BombEnemy(int bombID)
    {
        bombEnemyDetonatedStatus[bombID] = true;
    }

    public void SendBombEnemyDetonatedStatus()
    {
        string bombEnemyDetonatedStatusJson = "{";
        foreach (var kvp in bombEnemyDetonatedStatus)
        {
            bombEnemyDetonatedStatusJson += $"\"{kvp.Key}\": {(kvp.Value ? "true" : "false")}, ";
        }
        if (bombEnemyDetonatedStatusJson.EndsWith(", "))
        {
            bombEnemyDetonatedStatusJson = bombEnemyDetonatedStatusJson.Substring(0, bombEnemyDetonatedStatusJson.Length - 2);
        }
        bombEnemyDetonatedStatusJson += "}";
        RestClient.Post(firebaseURL + "playthroughs/" + currentLevel + "/bombEnemyDetonatedStatus.json", bombEnemyDetonatedStatusJson)
            .Then(response =>
            {
                Debug.Log("Successfully sent bomb enemy detonated status to Firebase for " + currentLevel);
            })
            .Catch(error =>
            {
                Debug.LogError("Error sending bomb enemy detonated status to Firebase: " + error.Message);
            });
    }

    public void SendKeySpacebarCount()
    {
        KeySpacebarCountJsonData keySpacebarCountData = new KeySpacebarCountJsonData { UpKeyClickCount = upKeyClickCount, SpacebarClickCount = spacebarClickCount };
        string keySpacebarCountJsonData = JsonUtility.ToJson(keySpacebarCountData);
        RestClient.Post(firebaseURL + "playthroughs/" + currentLevel + "/keySpacebarCount.json", keySpacebarCountJsonData)
            .Then(response =>
            {
                Debug.Log("Successfully sent key spacebar count to Firebase for " + currentLevel);
            })
            .Catch(error =>
            {
                Debug.LogError("Error sending key spacebar count to Firebase: " + error.Message);
            });
    }

    public void SendGunsCollectedCount()
    {
        CollectedGunCountJsonData collectedGunData = new CollectedGunCountJsonData { GunsCollectedCount = gunsCollectedCount };
        string collectedGunJsonData = JsonUtility.ToJson(collectedGunData);
        RestClient.Post(firebaseURL + "playthroughs/" + currentLevel + "/gunsCollectedCount.json", collectedGunJsonData)
            .Then(response =>
            {
                Debug.Log("Successfully sent guns collected count to Firebase for " + currentLevel);
            })
            .Catch(error =>
            {
                Debug.LogError("Error sending guns collected count to Firebase: " + error.Message);
            });
    }

    private void RecordNewPlayerSession()
    {
        GeneratePlaythroughId(); // Ensure a unique ID is generated for each session
        string sessionEndpoint = firebaseURL + "playerSessions.json";
        PlayerSessionData sessionData = new PlayerSessionData
        {
            StartTime = DateTime.UtcNow.ToString("o"), // ISO 8601 format
            SceneName = SceneManager.GetActiveScene().name // Storing the scene name
        };
        string sessionJsonData = JsonUtility.ToJson(sessionData);

        RestClient.Post(sessionEndpoint, sessionJsonData).Then(response =>
        {
            Debug.Log("New player session recorded for scene: " + sessionData.SceneName);
        }).Catch(error =>
        {
            Debug.LogError("Error recording new player session: " + error.Message);
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

    [System.Serializable]
    public class SwitchCountJsonData
    {
        public int SwitchCount;
    }
    [System.Serializable]
    private class ZoneTimeData
    {
        public float ZoneTime;
    }
    [System.Serializable]
    private class CollectedBombCountJsonData
    {
        public int BombsCollectedCount;
    }
    [System.Serializable]
    private class StarCountJsonData
    {
        public int StarCount;
    }
    [System.Serializable]
    private class BombsDetonatedCountJsonData
    {
        public int BombsDetonatedCount;
    }
    [System.Serializable]
    private class KeySpacebarCountJsonData
    {
        public int UpKeyClickCount;
        public int SpacebarClickCount;
    }
    [System.Serializable]
    private class CollectedGunCountJsonData
    {
        public int GunsCollectedCount;
    }
    [System.Serializable]
    private class PlayerSessionData
    {
        public string StartTime;
        public string SceneName;

    }
}
