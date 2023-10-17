using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBomb : MonoBehaviour
{
    [SerializeField] private BombThrower player;
    [SerializeField] private LevelColor color;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.bombColor = color; // TODO: Consider replacing with BombCollected(color) event
            Destroy(gameObject);
        }
    }

}
