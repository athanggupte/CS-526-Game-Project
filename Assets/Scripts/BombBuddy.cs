using System.Collections;
using UnityEngine;

public class BombBuddy : MonoBehaviour
{
    [SerializeField] private Sprite tileSprite;
    [SerializeField] private Sprite bombSprite;
    public int bombID = 0;

    void Start()
    {
        m_effector = GetComponent<ColorBombEffector>();
        m_colorEntity = GetComponent<ColorEntity>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(HideReveal());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LevelEvents.Instance.BombEnemyDetonate.Invoke(bombID);
            m_spriteRenderer.sprite = bombSprite;
            m_effector.Deploy();
            m_alive = false;
        }        
    }

    IEnumerator HideReveal()
    {
        while (m_alive)
        {
            m_spriteRenderer.sprite = tileSprite;
            m_colorEntity.ReapplyColor();
            yield return new WaitForSeconds(Random.RandomRange(1.5f, 3.0f));
            m_spriteRenderer.sprite = bombSprite;
            Color tmpColor = ServiceLocator.LevelColorController.GetTileColorRGB(m_effector.color);
            tmpColor.a = (m_colorEntity.IsActive()) ? 1.0f : 0.2f;
            m_spriteRenderer.color = tmpColor;
            yield return new WaitForSeconds(Random.RandomRange(0.5f, 1.5f));
        }
    }

    private ColorBombEffector m_effector;
    private ColorEntity m_colorEntity;
    private SpriteRenderer m_spriteRenderer;

    private bool m_alive = true;
}
