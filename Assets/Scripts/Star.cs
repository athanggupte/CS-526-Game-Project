using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private bool IsCollected = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsCollected)
        {
            IsCollected = true;
            if (collision.CompareTag("Player"))
            {
                LevelEvents.Instance.StarCollect.Invoke();
            }
        }
    }
}
