using UnityEngine;

public class BombBuddy : MonoBehaviour
{
    [SerializeField] private Sprite bombSprite;
    public int bombID = 0;

    void Start()
    {
        m_effector = GetComponent<ColorBombEffector>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LevelEvents.Instance.BombEnemyDetonate.Invoke(bombID);
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = bombSprite;
            m_effector.Deploy();
        }        
    }

    private ColorBombEffector m_effector;
}
