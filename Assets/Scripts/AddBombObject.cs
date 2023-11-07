using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBombObject : MonoBehaviour
{
    public LevelColor bombColor;
    public GameObject collectibleBombPrefab;

    private WeaponController weaponController;
    private GameObject collectibleBombInstance;

    void Start()
    {
        weaponController = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponController>();
    }

    void Update()
    {
        if (collectibleBombInstance == null && weaponController.BombHandler.AmmoCount == 0 && !weaponController.BombHandler.IsLastBombActive)
        {
            Debug.Log("Adding new collectible bomb");
            collectibleBombInstance = Instantiate(collectibleBombPrefab, transform);
            collectibleBombInstance.transform.localPosition = Vector3.zero;
            collectibleBombInstance.GetComponent<CollectibleBomb>().SetColor(bombColor);
        }
    }
}
