using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleGun : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelEvents.Instance.GunCollect.Invoke(ServiceLocator.ActiveZoneController.ActiveZoneName);
            Destroy(gameObject);
        }
    }
}
