using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    public GameObject missilePrefab;
    public LevelColor missileColor;
    public GameObject muzzle;
    public GameObject player;
    public float power = 5000f;
    public float firingDelay = 5;

    void Start()
    {
        StartCoroutine(ShootCoro());
    }

    private void Shoot()
    {
        Vector2 pos = new Vector2(muzzle.transform.position.x, muzzle.transform.position.y);

        Vector2 dir = ((Vector2)player.transform.position) - pos;
        dir = dir.normalized;

        Vector2 shootPos = pos + dir;

        GameObject missile = Instantiate(missilePrefab, shootPos, Quaternion.identity);

        var missileEffector = missile.GetComponent<MissileEffector>();
        missileEffector.color = missileColor;
        missileEffector.Deploy(dir * power);
    }

    IEnumerator ShootCoro()
    {
        while (true)
        {
            if (GetComponent<ColorEntity>().IsActive())
            {
                Shoot();
                yield return new WaitForSeconds(firingDelay);
            }
            else
            {
                yield return null;
            }
        }
    }

}
