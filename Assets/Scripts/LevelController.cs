using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
    }
}
