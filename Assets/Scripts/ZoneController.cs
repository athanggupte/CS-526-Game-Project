using UnityEngine;
using Proyecto26;

public class ZoneController : MonoBehaviour
{
    private const string playerTag = "Player";
    private const string firebaseURL = "https://hue-hustlers-default-rtdb.firebaseio.com/";
    public string zoneName = "Zone1";  // Set this in the Unity Inspector for each zone

    private string currentLevel => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    private bool hasZoneBeenTriggered = false;  // Add this line
    public DataCollector dataCollector;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && !hasZoneBeenTriggered)  // Check the flag here
        {
            Debug.Log("Player entered the 2D zone: " + zoneName);
            SendDataToFirebase();
            hasZoneBeenTriggered = true;  // Set the flag to true once data is sent
        }
    }

    private void SendDataToFirebase()
    {
        float currentTime = Time.time; // Get the current time since the game started

        // Adjusted endpoint to include zones
        string checkpointEndpoint = firebaseURL + "checkpointData/" + currentLevel + "/" + zoneName + ".json";
        int[] colorSwitchCounts = dataCollector.GetColorSwitchCount();
        DataCollector.ColorSwitchCountsData colorData = new DataCollector.ColorSwitchCountsData
        {
            Red = colorSwitchCounts[(int)LevelColor.Red],
            Blue = colorSwitchCounts[(int)LevelColor.Blue],
            Yellow = colorSwitchCounts[(int)LevelColor.Yellow]
        };
        LevelCompletionData checkpointData = new LevelCompletionData
        {
            CompletionTime = currentTime,
            colorSwitchCount = colorData
        };
        string checkpointJsonData = JsonUtility.ToJson(checkpointData);

        RestClient.Post(checkpointEndpoint, checkpointJsonData)
            .Then(response =>
            {
                Debug.Log("Successfully sent checkpoint time to Firebase for " + currentLevel + " in " + zoneName);
            })
            .Catch(error =>
            {
                Debug.LogError("Error sending checkpoint time to Firebase: " + error.Message);
            });
    }

    [System.Serializable]
    private class LevelCompletionData
    {
        public float CompletionTime;
        public DataCollector.ColorSwitchCountsData colorSwitchCount;
    }
}
