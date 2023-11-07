using UnityEngine;
using UnityEngine.Assertions;

public class MouseAiming : MonoBehaviour
{
    [SerializeField] private GameObject aimingReticle;
    [SerializeField] private GameObject colorIndicator;
    [SerializeField] private GameObject gun;

    public Vector3 CurrentDirection { get => m_currentDirection; }

    public Vector3 ThrowVector { get => m_currentDirection * m_reticleDistance; }

    public bool ShowReticle
    {
        get => m_showReticle;
        set
        {
            m_showReticle = value;
            aimingReticle.SetActive(value);
        }
    }

    public bool ShowGun
    {
        get => m_showGun;
        set
        {
            m_showGun = value;
            gun.SetActive(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_playerMovement = GetComponent<PlayerMovement>();
        m_weaponController = GetComponent<WeaponController>();

        ShowReticle = false;
        ShowGun = false;
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
                float minY = m_playerMovement.GroundCheckTransform().localPosition.y + 0.15f;
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

        if (ShowReticle)
        {
            aimingReticle.transform.position = transform.position + (m_currentDirection * m_reticleDistance) + new Vector3(0, 0, -1);
            aimingReticle.transform.up = m_currentDirection;

            colorIndicator.GetComponent<SpriteRenderer>().color = ServiceLocator.LevelColorController.GetTileColorRGB(m_weaponController.BombHandler.CurrentBombColor);

            Assert.AreApproximatelyEqual((aimingReticle.transform.position - transform.position).magnitude, Mathf.Sqrt(2));
        }

        if (ShowGun)
        {
            gun.transform.position = transform.position + (m_currentDirection * m_reticleDistance);
            gun.transform.up = m_currentDirection;

            Vector3 localScale = gun.transform.localScale;
            int sign = Vector3.Dot(m_currentDirection, transform.right * transform.localScale.x) < 0 ? -1 : 1;
            localScale.x = sign * Mathf.Abs(localScale.x);
            gun.transform.localScale = localScale;
                
            gun.GetComponent<SpriteRenderer>().color = ServiceLocator.LevelColorController.GetTileColorRGB(ServiceLocator.LevelColorController.CurrentColor);
        }
    }

    private WeaponController m_weaponController;

    private Vector3 m_currentDirection;
    private PlayerMovement m_playerMovement;
    private float m_reticleDistance = 1.0f;
    private bool m_showReticle;
    private bool m_showGun;
}
