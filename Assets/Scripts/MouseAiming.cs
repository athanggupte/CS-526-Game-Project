using UnityEngine;
using UnityEngine.Assertions;

public class MouseAiming : MonoBehaviour
{
    [SerializeField] private GameObject aimingReticle;

    public Vector3 CurrentDirection { get => m_currentDirection; }

    public Vector3 ThrowVector { get => m_currentDirection * m_reticleDistance; }

    // Start is called before the first frame update
    void Start()
    {
        m_playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 transformPosition = new Vector3(transform.position.x, transform.position.y);

        m_currentDirection = mousePosition - transformPosition;
        if (m_playerMovement.IsGrounded())
        {
            if (m_currentDirection.y < 0)
            {
                float minY = m_playerMovement.GroundCheckTransform().localPosition.y + 0.2f;
                m_currentDirection.y = Mathf.Max(m_currentDirection.y, minY);
                //Debug.Log("minY : " + minY);

                float minAbsX = Mathf.Sqrt(1 - minY * minY);
                Debug.DrawLine(transformPosition, transformPosition + new Vector3(minAbsX, minY, 0));

                if (m_currentDirection.x > 0)
                {
                    m_currentDirection.x = Mathf.Max(m_currentDirection.x, minAbsX);
                }
                else
                {
                    m_currentDirection.x = Mathf.Min(m_currentDirection.x, -minAbsX);
                }
            }
        }
        m_currentDirection.Normalize();

        if (aimingReticle)
        {
            aimingReticle.transform.position = transform.position + (m_currentDirection * m_reticleDistance) + new Vector3(0, 0, -1);
            aimingReticle.transform.up = m_currentDirection;

            Assert.AreApproximatelyEqual((aimingReticle.transform.position - transform.position).magnitude, Mathf.Sqrt(2));
        }
    }

    private Vector3 m_currentDirection;
    private PlayerMovement m_playerMovement;
    private float m_reticleDistance = 1.0f;
}
