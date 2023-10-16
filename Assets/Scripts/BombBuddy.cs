using UnityEngine;

public class BombBuddy : MonoBehaviour
{
    [SerializeField] private Sprite bombSprite;

    void Start()
    {
        m_effector = GetComponent<ColorBombEffector>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = bombSprite;
            m_effector.Deploy();
        }        
    }

    private ColorBombEffector m_effector;
}
