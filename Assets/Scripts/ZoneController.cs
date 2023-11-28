using UnityEditor;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    private const string playerTag = "Player";
    public string zoneName = "Zone1";  // Set this in the Unity Inspector for each zone

    private float zoneEntryTime;  // Time when the player entered the zone
    private float totalSpentTime = 0f;  // Cumulative time spent in the zone

    private Color m_gizmoColor;
    private bool m_initGizmos = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            ServiceLocator.ActiveZoneController.ActiveZone = this;
            zoneEntryTime = Time.time;
            //Debug.Log("Player entered the 2D zone: " + zoneName);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            ServiceLocator.ActiveZoneController.ActiveZone = null;
            float duration = Time.time - zoneEntryTime;
            totalSpentTime += duration;

            // Update the time in DataCollector
            ServiceLocator.DataCollector.UpdateZoneTime(zoneName, duration);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!m_initGizmos)
        {
            m_gizmoColor = Random.ColorHSV(0f, 1f, 0f, 1f, 0.8f, 1f, 0.4f, 0.5f);
            m_initGizmos = true;
        }
        Gizmos.color = m_gizmoColor;
        Gizmos.DrawCube(transform.position, transform.localScale);
        
        var textLocalPos = new Vector3(-0.95f, 0.85f, 0);
        textLocalPos.Scale(transform.localScale/2);
        Handles.Label(transform.position + textLocalPos, "Zone: " + zoneName);
        //Debug.Log(m_gizmoColor);
    }
}
