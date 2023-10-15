using UnityEngine;

public class BombBuddy : MonoBehaviour
{
    public Sprite BombSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = BombSprite;
            gameObject.GetComponent<ColorBombEffector>().Deploy();
        }        
    }
}
