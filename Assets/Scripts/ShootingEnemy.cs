using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    private float delay;
    public GameObject bullet;
    public GameObject muzzle;
    public GameObject player;
    public float power = 5f;
    public float firerate = 5000f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }
    private void Shoot()
    {
        if(Time.time> delay)
        {
            delay = Time.time + firerate / 1000;
            Vector2 pos = new Vector2(muzzle.transform.position.x, muzzle.transform.position.y);
            GameObject bomb = Instantiate(bullet, pos, Quaternion.identity);
            Vector2 dir = ((Vector2)player.transform.position).normalized;
            bomb.GetComponent<Rigidbody2D>().velocity = dir * power;
            Destroy(bomb, 3f);
        }
        

    }

}
