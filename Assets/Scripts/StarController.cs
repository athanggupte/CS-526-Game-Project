using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    [SerializeField]
    private int StarCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        LevelEvents.Instance.StarCollect.AddListener(CollectStar);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CollectStar()
    {
        StarCount++;
    }
}
