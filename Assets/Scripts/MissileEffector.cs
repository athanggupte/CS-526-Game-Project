using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissileEffector : MonoBehaviour
{
    public LevelColor color;
    public float radius;

    private Rigidbody2D rb;

    public void Deploy(Vector3 velocity)
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = velocity;
        GetComponent<PolygonCollider2D>().enabled = false;

        transform.up = velocity;

        foreach (var childSpriteRenderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
            childSpriteRenderer.color = ServiceLocator.LevelColorController.GetTileColorRGB(color);

        StartCoroutine(EnableCollider(0.5f));
        
        StartCoroutine(TimedDetonate());
    }

    IEnumerator EnableCollider(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<PolygonCollider2D>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        LevelEvents.Instance.ColorBombDetonate.Invoke(color, transform.position, radius, ServiceLocator.ActiveZoneController.ActiveZoneName);
        Destroy(gameObject);
    }

    IEnumerator TimedDetonate()
    {
        yield return new WaitForSeconds(3);
        LevelEvents.Instance.ColorBombDetonate.Invoke(color, transform.position, radius, ServiceLocator.ActiveZoneController.ActiveZoneName);
        Destroy(gameObject);
    }

}
