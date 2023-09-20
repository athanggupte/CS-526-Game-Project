using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiminishingColor : MonoBehaviour
{
    public float diminishingRate = 0.02f;
    private float dimFactor;

    // Start is called before the first frame update
    void Start()
    {
        dimFactor = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        dimFactor -= Time.deltaTime * diminishingRate;
    }
}
