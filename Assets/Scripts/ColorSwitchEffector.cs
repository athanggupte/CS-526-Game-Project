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
        if (collision.CompareTag("Player"))
        {
            LevelEvents.Instance.ColorSwitch.Invoke(targetColor);

            // levelColorController.CurrentColor = targetColor;
            //Orb orb = gameObject.GetComponent<Orb>();
            //string currentOrbID = orb.orbID;
            //if (currentOrbID != orbController.previousOrbID)
            //{
            //    dataCollector.CollectColorSwitch(targetColor);
            //}
            //orbController.previousOrbID = currentOrbID;

        }
    }
}
