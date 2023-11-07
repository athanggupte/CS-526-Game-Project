using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFlash : MonoBehaviour
{
    public float screenTimeMillis = 400f;
    public LevelColor color;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Bomb Flash Started!");
        GetComponent<SpriteRenderer>().color = ServiceLocator.LevelColorController.GetTileColorRGB(color);
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(screenTimeMillis / 1000f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
