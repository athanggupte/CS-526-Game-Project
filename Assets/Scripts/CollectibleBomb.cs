using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBomb : MonoBehaviour
{
    [SerializeField] private LevelColor color;

    public void SetColor(LevelColor color)
    {
        this.color = color;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = ServiceLocator.LevelColorController.GetTileColorRGB(color);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelEvents.Instance.BombCollect.Invoke(color, ServiceLocator.ActiveZoneController.activeZoneName);
            Destroy(gameObject);
        }
    }
}
