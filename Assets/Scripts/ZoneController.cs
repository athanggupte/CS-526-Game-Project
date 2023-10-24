using UnityEngine;

public class ZoneController : MonoBehaviour
{
    private const string playerTag = "Player";
    public string zoneName = "Zone1";  // Set this in the Unity Inspector for each zone

    private float zoneEntryTime;  // Time when the player entered the zone
    private float totalSpentTime = 0f;  // Cumulative time spent in the zone

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            zoneEntryTime = Time.time;
            Debug.Log("Player entered the 2D zone: " + zoneName);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            float duration = Time.time - zoneEntryTime;
            totalSpentTime += duration;

            // Update the time in DataCollector
            ServiceLocator.DataCollector.UpdateZoneTime(zoneName, duration);
        }
    }
}
