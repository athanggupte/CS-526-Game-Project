using UnityEngine;
using Proyecto26;

public class TimeSendingZone : MonoBehaviour
{
    private const string playerTag = "Player";
    private const string firebaseURL = "https://hue-hustlers-default-rtdb.firebaseio.com/";
    public string zoneName = "Zone1";  // Set this in the Unity Inspector for each zone

    private string currentLevel => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    private bool hasZoneBeenTriggered = false;  // Add this line

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && !hasZoneBeenTriggered)  // Check the flag here
        {
            Debug.Log("Player entered the 2D zone: " + zoneName);
            SendTimeToFirebase();
            hasZoneBeenTriggered = true;  // Set the flag to true once data is sent
        }
    }

    private void SendTimeToFirebase()
    {
        float currentTime = Time.time; // Get the current time since the game started

        // Adjusted endpoint to include zones
        string checkpointEndpoint = firebaseURL + "checkpointData/" + currentLevel + "/" + zoneName + ".json";

        LevelCompletionData checkpointData = new LevelCompletionData
        {
            CompletionTime = currentTime
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
    }
}
