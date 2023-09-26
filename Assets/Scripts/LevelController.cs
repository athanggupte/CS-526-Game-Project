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
            bool layerEnabled = (i == (int)level);

            layers[i].GetComponent<TilemapCollider2D>().enabled = layerEnabled;

            Vector3 tmpPos = layers[i].transform.position;
            layers[i].transform.position.Set(tmpPos.x, tmpPos.y, layerEnabled ? 1 : 0);

            Color tmpColor = layers[i].GetComponent<Tilemap>().color;
            tmpColor.a = (i == (int)level) ? 1.0f : 0.2f;
            layers[i].GetComponent<Tilemap>().color = tmpColor;
        }
    }
}
