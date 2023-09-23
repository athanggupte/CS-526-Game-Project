using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelController : MonoBehaviour
{
    public enum Level
    {
        Red,
        Yellow,
        Blue
    };

    public Level level = Level.Red;

    public GameObject[] layers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].SetActive(i == (int)level);
            //Color tmp = layers[i].GetComponent<Tilemap>().color;
            //tmp.a = (i == (int)level) ? 1.0f : 0.2f;
            //layers[i].GetComponent<Tilemap>().color = tmp;
        }
    }
}
