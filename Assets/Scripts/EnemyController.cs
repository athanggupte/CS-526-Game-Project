using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Tilemaps;
using static LevelColorController;

public class EnemyController : MonoBehaviour
{
    public LevelColorController.Level selfColor;
    public LevelColorController levelController;

    private bool active;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        active = (levelController.level == selfColor);

        Color tmpColor = GetComponent<SpriteRenderer>().color;
        tmpColor.a = (active) ? 1.0f : 0.2f;
        GetComponent<SpriteRenderer>().color = tmpColor;
    }

    public bool IsActive()
    {
        return active;
    }
}
