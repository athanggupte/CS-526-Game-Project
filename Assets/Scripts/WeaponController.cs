using System;
using TMPro;
using UnityEngine;

public enum Weapon
{
    None,
    Bomb,
    Gun
}

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject contextualHintTextPrefab;
    [SerializeField] private BombWeaponHandler bombHandler;
    [SerializeField] private GunWeaponHandler gunHandler;
    [SerializeField] private Weapon activeWeapon;

    public const int MAX_BOMB_AMMO = 3;
    public const int MAX_GUN_AMMO = 6;

    public Weapon ActiveWeapon { get => activeWeapon; }
    public BombWeaponHandler BombHandler { get => bombHandler; }
    public GunWeaponHandler GunHandler { get => gunHandler; }

    void Awake()
    {
        LevelEvents.LevelEventsInitialized.AddListener(ListenToLevelEvents);
    }

    private void ListenToLevelEvents()
    {
        LevelEvents.Instance.BombCollect.AddListener(CollectBomb);
    }

    void Start()
    {
        m_mouseAiming = GetComponent<MouseAiming>();

        bombHandler.gameObject.SetActive(false);
        gunHandler.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            activeWeapon = Weapon.Bomb;
            bombHandler.gameObject.SetActive(true);
            gunHandler.gameObject.SetActive(false);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            activeWeapon = Weapon.Gun;
            bombHandler.gameObject.SetActive(false);
            gunHandler.gameObject.SetActive(true);
        }

        if (activeWeapon == Weapon.Bomb)
        {
            m_mouseAiming.ShowReticle = true;
            m_mouseAiming.ShowGun = false;

            if (Input.GetMouseButtonDown(0 /* LMB */))
            {
                if (!bombHandler.HasBomb())
                {
                    if (m_lastNoBombsCollectedTooltip == null)
                    {
                        m_numClicksBeforeCollectBombMessage -= 1;
                        if (m_numClicksBeforeCollectBombMessage == 0)
                        {
                            m_numClicksBeforeCollectBombMessage = 3;
                            GameObject textGo = Instantiate(contextualHintTextPrefab);
                            textGo.GetComponent<TextMeshPro>().text = "No bombs collected";
                            ContextualTooltip tooltip = textGo.GetComponent<ContextualTooltip>();
                            tooltip.StickToTarget(gameObject, new Vector3(0, 3, 2));
                            tooltip.Deploy(2.5f);

                            m_lastNoBombsCollectedTooltip = textGo;
                        }
                    }
                }
                else if (bombHandler.IsLastBombActive)
                {
                    bombHandler.DetonateBomb();
                }
                else
                {
                    bombHandler.ThrowBomb(m_mouseAiming.ThrowVector);
                }
            }
        }
        else if (activeWeapon == Weapon.Gun)
        {
            m_mouseAiming.ShowReticle = false;
            m_mouseAiming.ShowGun = true;

            if (Input.GetMouseButtonDown(0 /* LMB */))
            {
                gunHandler.Shoot();
            }
        }
    }
    
    private void CollectBomb(LevelColor color)
    {
        bombHandler.SetAmmo(MAX_BOMB_AMMO, color);
    }

    private MouseAiming m_mouseAiming;

    private int m_numClicksBeforeCollectBombMessage = 3;
    private GameObject m_lastNoBombsCollectedTooltip;
}
