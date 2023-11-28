using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFlash : MonoBehaviour
{
    public float growTimeMillis = 200f;
    public float screenTimeMillis = 400f;
    public float targetScale;
    public LevelColor color;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Bomb Flash Started!");
        GetComponent<SpriteRenderer>().color = ServiceLocator.LevelColorController.GetTileColorRGB(color);
        StartCoroutine(DestroyAfterDelay());
        timer = 0;
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(screenTimeMillis / 1000f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < growTimeMillis)
        {
            float t = timer / (growTimeMillis / 1000f);
            t = t * t * t;
            transform.localScale = Vector3.one * Mathf.Lerp(0, targetScale, t);
        }
        timer += Time.deltaTime;
    }
}
