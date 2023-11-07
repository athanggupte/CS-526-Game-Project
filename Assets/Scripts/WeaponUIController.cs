using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class WeaponUIController : MonoBehaviour
{
    [SerializeField] Sprite BombSprite;
    [SerializeField] Sprite GunSprite;

    private int NUM_AMMO = 6;

    void Start()
    {
        m_weaponController = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponController>();

        m_ammoCollection = transform.GetChild(0).gameObject.GetComponent<RectTransform>();

        m_weaponIndicator = transform.GetChild(1).gameObject;
        m_weaponIndicatorImage = m_weaponIndicator.transform.GetChild(0).GetComponent<Image>();
        
        Assert.IsNotNull(m_weaponController);
        Assert.IsNotNull(m_weaponIndicator);

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
        Color currentWeaponColor = m_weaponController.ActiveWeapon == Weapon.Bomb ? 
            ServiceLocator.LevelColorController.GetTileColorRGB(m_weaponController.BombHandler.CurrentBombColor) :
            ServiceLocator.LevelColorController.GetTileColorRGB(ServiceLocator.LevelColorController.CurrentColor);
        Sprite currentWeaponSprite = m_weaponController.ActiveWeapon == Weapon.Bomb ? BombSprite : GunSprite;

        int ammoCollectionHeight = m_weaponController.ActiveWeapon == Weapon.Bomb ? 160 : 280;
        m_ammoCollection.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ammoCollectionHeight);

        m_weaponIndicatorImage.color = currentWeaponColor;
        m_weaponIndicatorImage.sprite = currentWeaponSprite;

        int ammoMaxCount = m_weaponController.ActiveWeapon == Weapon.Bomb ? 3 : 6;
        for (int i = 0; i < ammoMaxCount; i++)
        {
            m_ammoIndicators[i].SetActive(true);
        }
        for (int i = ammoMaxCount; i < NUM_AMMO; i++)
        {
            m_ammoIndicators[i].SetActive(false);
        }

        for (int i = 0; i < m_weaponController.ActiveWeaponAmmoCount; i++)
        {
            m_ammoIndicators[i].transform.GetChild(0).gameObject.SetActive(true);
            m_ammoIndicatorImages[i].color = currentWeaponColor;
            m_ammoIndicatorImages[i].sprite = currentWeaponSprite;
        }
        for (int i = m_weaponController.ActiveWeaponAmmoCount; i < NUM_AMMO; i++)
        {
            m_ammoIndicators[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private WeaponController m_weaponController;
    private RectTransform m_ammoCollection;
    private GameObject m_weaponIndicator;
    private GameObject[] m_ammoIndicators;
    private Image m_weaponIndicatorImage;
    private Image[] m_ammoIndicatorImages;

}
