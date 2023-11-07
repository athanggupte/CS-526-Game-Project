using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBomb : MonoBehaviour
{
    [SerializeField] private LevelColor color;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelEvents.Instance.BombCollect.Invoke(color);
            Destroy(gameObject);
        }
    }
}
