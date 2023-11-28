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
    [SerializeField] private LayerMask groundLayer;

    public const int MAX_BOMB_AMMO = 3;
    public const int MAX_GUN_AMMO = 4;

    public Weapon ActiveWeapon
    {
        get => activeWeapon;
        private set
        {
            activeWeapon = value;
            switch (activeWeapon)
            {
                case Weapon.None:
                    bombHandler.gameObject.SetActive(false);
                    gunHandler.gameObject.SetActive(false);
                    m_mouseAiming.ShowReticle = false;
                    m_mouseAiming.ShowGun = false;
                    break;
                case Weapon.Bomb:
                    bombHandler.gameObject.SetActive(true);
                    gunHandler.gameObject.SetActive(false);
                    m_mouseAiming.ShowReticle = true;
                    m_mouseAiming.ShowGun = false;
                    break;
                case Weapon.Gun:
                    bombHandler.gameObject.SetActive(false);
                    gunHandler.gameObject.SetActive(true);
                    m_mouseAiming.ShowReticle = false;
                    m_mouseAiming.ShowGun = true;
                    break;
            }
        }
    }
    public int ActiveWeaponAmmoCount {
        get {
            switch (activeWeapon) {
                case Weapon.Bomb: return bombHandler.AmmoCount;
                case Weapon.Gun: return gunHandler.AmmoCount; 
            }
            return 0;
        }
    }
    public BombWeaponHandler BombHandler { get => bombHandler; }
    public GunWeaponHandler GunHandler { get => gunHandler; }

    void Awake()
    {
        LevelEvents.LevelEventsInitialized.AddListener(ListenToLevelEvents);
    }

    private void ListenToLevelEvents()
    {
        LevelEvents.Instance.BombCollect.AddListener(CollectBomb);
        LevelEvents.Instance.GunCollect.AddListener(CollectGun);
    }

    void Start()
    {
        m_mouseAiming = GetComponent<MouseAiming>();

        var audioSource = GetComponent<AudioSource>();
        ServiceLocator.AudioSource = audioSource;

        bombHandler.gameObject.SetActive(false);
        gunHandler.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            ActiveWeapon = activeWeapon == Weapon.Bomb ? Weapon.None : Weapon.Bomb;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            ActiveWeapon = activeWeapon == Weapon.Gun ? Weapon.None : Weapon.Gun;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ActiveWeapon = activeWeapon == Weapon.Bomb ? Weapon.Gun : Weapon.Bomb;
        }



        if (Input.GetMouseButtonDown(0 /* LMB */))
        {
            switch (activeWeapon)
            {
                case Weapon.None:
                    break;
                case Weapon.Bomb:
                    if (bombHandler.IsLastBombActive)
                    {
                        bombHandler.DetonateBomb();
                    }
                    else if (bombHandler.AmmoCount > 0)
                    {
                        if (!CheckOverlap()) bombHandler.ThrowBomb();
                    }
                    else // No Ammo
                    {
                        LevelEvents.Instance.NoAmmoBomb.Invoke();
                        if (m_noAmmoTooltip == null)
                        {
                            m_numClicksBeforeCollectBombMessage -= 1;
                            if (m_numClicksBeforeCollectBombMessage == 0)
                            {
                                m_numClicksBeforeCollectBombMessage = 3;
                                GameObject textGo = Instantiate(contextualHintTextPrefab);
                                textGo.GetComponent<TextMeshPro>().text = "No Ammo";
                                ContextualTooltip tooltip = textGo.GetComponent<ContextualTooltip>();
                                tooltip.StickToTarget(gameObject, new Vector3(0, 3, 2));
                                tooltip.Deploy(2.5f);

                                m_noAmmoTooltip = textGo;
                            }
                        }
                    }
                    break;
                case Weapon.Gun:
                    if (gunHandler.AmmoCount > 0)
                    {
                        if (!CheckOverlap()) gunHandler.Shoot();
                    }
                    else // No Ammo
                    {
                        LevelEvents.Instance.NoAmmoGun.Invoke();
                        if (m_noAmmoTooltip == null)
                        {
                            m_numClicksBeforeCollectBombMessage -= 1;
                            if (m_numClicksBeforeCollectBombMessage == 0)
                            {
                                m_numClicksBeforeCollectBombMessage = 3;
                                GameObject textGo = Instantiate(contextualHintTextPrefab);
                                textGo.GetComponent<TextMeshPro>().text = "No Ammo";
                                ContextualTooltip tooltip = textGo.GetComponent<ContextualTooltip>();
                                tooltip.StickToTarget(gameObject, new Vector3(0, 3, 2));
                                tooltip.Deploy(2.5f);

                                m_noAmmoTooltip = textGo;
                            }
                        }
                    }
                    break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position + m_mouseAiming.ThrowVector, 0.25f);
    }

    public int GetWeaponAmmoCount(Weapon weaponType)
    {
        switch (weaponType)
        {
            case Weapon.Bomb: return bombHandler.AmmoCount;
            case Weapon.Gun: return gunHandler.AmmoCount;
        }
        return 0;
    }

    private void CollectBomb(LevelColor color, string zoneName)
    {
        bombHandler.SetAmmo(MAX_BOMB_AMMO, color);
    }

    private void CollectGun(string zoneName)
    {
        gunHandler.SetAmmo(MAX_GUN_AMMO);
    }

    private bool CheckOverlap()
    {
        return Physics2D.OverlapCircle(transform.position + m_mouseAiming.ThrowVector, 0.25f, groundLayer);
    }

    private MouseAiming m_mouseAiming;

    private int m_numClicksBeforeCollectBombMessage = 3;
    private GameObject m_noAmmoTooltip;
}
