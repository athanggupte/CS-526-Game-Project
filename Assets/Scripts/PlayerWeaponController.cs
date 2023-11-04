using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    None,
    Bomb,
    Gun
}

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Weapon m_activeWeapon;

    public Weapon ActiveWeapon { get => m_activeWeapon; }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            m_activeWeapon = Weapon.Bomb;
        }
    }

    public void ShootWeapon()
    {

    }

}
