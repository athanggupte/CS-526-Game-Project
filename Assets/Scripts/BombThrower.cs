using UnityEngine;

public class BombThrower : MonoBehaviour
{
    public GameObject bombPrefab;
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var bomb = Instantiate(bombPrefab);
            bomb.transform.position = transform.position + new Vector3(1,1,0);
            
            bomb.GetComponent<ColorBombEffector>().levelColorController = ServiceLocator.LevelColorController;

            var rb = bomb.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector3(speed, speed);
            
            bomb.GetComponent<ColorBombEffector>().Deploy();
        }
    }
}
