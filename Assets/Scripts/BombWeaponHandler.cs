using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class BombWeaponHandler : MonoBehaviour
{
    public LayerMask mask;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private LevelColor bombColor;
    [SerializeField] private float speed;

    private int ammoCount = 0;

    public bool IsLastBombActive
    {
        get => m_lastBomb;
    }

    public void SetAmmo(int count, LevelColor bomb_color)
    {
        ammoCount = count;
        bombColor = bomb_color;
    }

    public bool HasBomb() { return ammoCount > 0; }

    public LevelColor CurrentBombColor() { return bombColor; }

    void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        
        m_mouseAiming = GetComponentInParent<MouseAiming>();
    }

    void Update()
    {
        DrawTrajectory();
    }

    public void ThrowBomb(Vector3 throwVector)
    {
        if (ammoCount == 0)
            return;

        ammoCount--;
        
        m_lastBomb = Instantiate(bombPrefab);
        m_lastBomb.transform.position = transform.position + throwVector;
        m_lastBomb.GetComponent<ColorBombEffector>().color = bombColor;

        var rb = m_lastBomb.GetComponent<Rigidbody2D>();
        rb.velocity = throwVector * speed;
            
        m_lastBomb.GetComponent<ColorBombEffector>().Deploy();
    }

    public void DetonateBomb()
    {
        m_lastBomb.GetComponent<ColorBombEffector>().Detonate();
    }

    private void DrawTrajectory()
    {
        const int NumSegments = 12;

        Vector3[] segments = new Vector3[NumSegments];

        Vector3 velocity = m_mouseAiming.ThrowVector * speed;
        Vector3 acceleration = bombPrefab.GetComponent<Rigidbody2D>().gravityScale * Physics2D.gravity;

        segments[0] = transform.position + m_mouseAiming.ThrowVector;
        for (int i = 1; i < NumSegments; i++)
        {
            // p1 = p0 + ut + 1/2 at^2
            float time = Time.fixedDeltaTime * i * 10f;
            segments[i] = segments[0] + velocity * time + 0.5f * acceleration * time * time;
        }

        m_lineRenderer.SetPositions(segments);
        var color = ServiceLocator.LevelColorController.GetTileColorRGB(bombColor);
        m_lineRenderer.startColor = color;
        m_lineRenderer.endColor = color;
    }

    private GameObject m_lastBomb;
    private LineRenderer m_lineRenderer;
    private MouseAiming m_mouseAiming;
}
