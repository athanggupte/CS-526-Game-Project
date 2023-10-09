using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public LevelColorController.Level targetColor;
    public LevelColorController levelColorController;
    public DataCollector dataCollector;

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
            levelColorController.level = targetColor;
            dataCollector.CollectColorSwitch(targetColor);
        }
    }
}
