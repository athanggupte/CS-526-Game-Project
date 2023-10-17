using System.Drawing;
using UnityEngine;

public class BombThrower : MonoBehaviour
{
    public GameObject bombPrefab;
    public LevelColor bombColor;
    public float speed;

    public bool IsLastBombActive
    {
        get => m_lastBomb;
    }

    public void ThrowBomb(Vector3 throwVector)
    {
        m_lastBomb = Instantiate(bombPrefab);
        m_lastBomb.transform.position = transform.position + throwVector;
        m_lastBomb.GetComponent<ColorBombEffector>().color = bombColor;

        var rb = m_lastBomb.GetComponent<Rigidbody2D>();
        rb.velocity = throwVector * speed;
            
        m_lastBomb.GetComponent<ColorBombEffector>().Deploy();
    }

    public void DetonateBomb()
    {
        if (m_lastBomb != null)
        {
            m_lastBomb.GetComponent<ColorBombEffector>().Detonate();
        }
    }

    private GameObject m_lastBomb;
}
