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
    }

    public void ThrowBomb(Vector3 throwVector)
    {
        var bomb = Instantiate(bombPrefab);
        bomb.transform.position = transform.position + throwVector;
            
        bomb.GetComponent<ColorBombEffector>().levelColorController = ServiceLocator.LevelColorController;

        var rb = bomb.GetComponent<Rigidbody2D>();
        rb.velocity = throwVector * speed;
            
        bomb.GetComponent<ColorBombEffector>().Deploy();
    }
}
