using System.Drawing;
using UnityEngine;

public class BombThrower : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private LevelColor bombColor;
    [SerializeField] private float speed;

    public bool IsLastBombActive
    {
        get => m_lastBomb;
    }

    public void SetBombColor(LevelColor bomb_color)
    {
        m_hasBomb = true;
        bombColor = bomb_color;
    }

    public bool HasBomb() { return m_hasBomb; }
    public LevelColor CurrentBombColor() { return bombColor; }

    public void ThrowBomb(Vector3 throwVector)
    {
        if (m_hasBomb)
        {
            m_lastBomb = Instantiate(bombPrefab);
            m_lastBomb.transform.position = transform.position + throwVector;
            m_lastBomb.GetComponent<ColorBombEffector>().color = bombColor;

            var rb = m_lastBomb.GetComponent<Rigidbody2D>();
            rb.velocity = throwVector * speed;
            
            m_lastBomb.GetComponent<ColorBombEffector>().Deploy();
        }
    }

    public void DetonateBomb()
    {
        if (m_lastBomb != null)
        {
            m_lastBomb.GetComponent<ColorBombEffector>().Detonate();
        }
    }

    private bool m_hasBomb = false;
    private GameObject m_lastBomb;
}
