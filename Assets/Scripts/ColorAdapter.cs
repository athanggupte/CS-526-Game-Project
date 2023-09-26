using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorAdapter : MonoBehaviour
{
    public LevelColorController levelColorController;
    public Color[] colors;

    private Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        tilemap.color = colors[(int)levelColorController.level];
    }
}
