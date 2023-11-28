using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class WeaponUIController : MonoBehaviour
{
    [SerializeField] Sprite sprite;
    [SerializeField] Weapon weaponType;

    private int NUM_AMMO;

    void Start()
    {
        NUM_AMMO = weaponType == Weapon.Bomb ? WeaponController.MAX_BOMB_AMMO : WeaponController.MAX_GUN_AMMO;
        m_weaponController = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponController>();

        m_weaponIndicator = transform.GetChild(1).gameObject;
        m_weaponIndicatorImage = m_weaponIndicator.transform.GetChild(0).GetComponent<Image>();

        m_ammoCollection = transform.GetChild(0).gameObject;
        m_ammoCollectionImage = m_ammoCollection.GetComponent<Image>();

        m_bombBackgroundImage = transform.parent.GetChild(0).GetComponent<Image>();

        m_weaponUI = transform.parent.GetComponent<RectTransform>();

        m_ammoIndicators = new GameObject[NUM_AMMO];
        m_ammoIndicatorImages = new Image[NUM_AMMO];
        for (int i = 0; i < NUM_AMMO;  i++)
        {
            m_ammoIndicators[i] = transform.GetChild(i + 2).gameObject;
            m_ammoIndicatorImages[i] = m_ammoIndicators[i].transform.GetChild(0).GetComponent<Image>();

            Assert.IsNotNull(m_ammoIndicators[i]);
        }

    }

    void Update()
    {
        Color currentWeaponColor = Color.white;
        
        if (weaponType == Weapon.Bomb)
        {
            currentWeaponColor = ServiceLocator.LevelColorController.GetTileColorRGB(m_weaponController.BombHandler.CurrentBombColor);
        } 
        else if (weaponType == Weapon.Gun)
        {
            currentWeaponColor = ServiceLocator.LevelColorController.GetTileColorRGB(ServiceLocator.LevelColorController.CurrentColor);
        }

        m_weaponIndicatorImage.color = currentWeaponColor;

        Color backgroundColor = m_ammoCollectionImage.color;
        if (m_weaponController.ActiveWeapon != weaponType)
        {
            m_weaponUI.localScale = new Vector3(0.75f, 0.75f, 1);

            backgroundColor.a = 0.7f;
            m_ammoCollectionImage.color = backgroundColor;
            m_bombBackgroundImage.color = backgroundColor;
        }
        else
        {
            m_weaponUI.localScale = new Vector3(1.0f, 1.0f, 1);

            backgroundColor.a = 1.0f;
            m_ammoCollectionImage.color = backgroundColor;
            m_bombBackgroundImage.color = backgroundColor;
        }

        for (int i = 0; i < m_weaponController.GetWeaponAmmoCount(weaponType); i++)
        {
            m_ammoIndicators[i].transform.GetChild(0).gameObject.SetActive(true);
            m_ammoIndicatorImages[i].color = currentWeaponColor;
        }
        for (int i = m_weaponController.GetWeaponAmmoCount(weaponType); i < NUM_AMMO; i++)
        {
            m_ammoIndicators[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private WeaponController m_weaponController;
    private GameObject m_ammoCollection;
    private Image m_ammoCollectionImage;
    private Image m_bombBackgroundImage;
    private GameObject m_weaponIndicator;
    private GameObject[] m_ammoIndicators;
    private Image m_weaponIndicatorImage;
    private Image[] m_ammoIndicatorImages;
    private RectTransform m_weaponUI;
}
