using UnityEngine;

public class ColorSwitchEffector : MonoBehaviour
{
    public LevelColor targetColor;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && ServiceLocator.LevelColorController.CurrentColor != targetColor)
        {
            Orb orb = gameObject.GetComponent<Orb>();
            string currentOrbID = orb.orbID;
            if (currentOrbID != ServiceLocator.OrbController.previousOrbID)
            {
                Debug.Log(ServiceLocator.ActiveZoneController.activeZoneName);
                LevelEvents.Instance.ColorSwitch.Invoke(targetColor, ServiceLocator.ActiveZoneController.activeZoneName);
                LevelEvents.Instance.OrbColorSwitch.Invoke(targetColor, ServiceLocator.ActiveZoneController.activeZoneName);
            }
            ServiceLocator.OrbController.previousOrbID = currentOrbID;

        }
    }
}
