using UnityEngine;

public class GunWeaponHandler : MonoBehaviour
{
    public LayerMask mask;
    [SerializeField] private BombFlash bombFlashPrefab;
    [SerializeField] private float radius;

    [SerializeField] private int ammoCount = 0;

    public int AmmoCount { get => ammoCount; }

    void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();

        m_mouseAiming = GetComponentInParent<MouseAiming>();
    }

    void Update()
    {
        DrawTrajectory();
    }

    public void SetAmmo(int count)
    {
        ammoCount = count;
    }

    public void Shoot()
    {
        if (ammoCount == 0)
            return;

        ammoCount--;

        var color = ServiceLocator.LevelColorController.CurrentColor;

        var bombFlash = Instantiate(bombFlashPrefab);
        bombFlash.transform.SetPositionAndRotation(m_raycastHitPoint, Quaternion.identity);
        bombFlash.transform.localScale = Vector3.one * (0.5f + radius * 2);
        bombFlash.color = color;

        Vector3 shootPoint = m_raycastHitPoint;
        if (Vector3.Dot(m_mouseAiming.ThrowVector, Vector3.up) < 0)
        {
            Debug.Log("pointing downwards");
            shootPoint -= new Vector3(0, 1f, 0);
        }

        LevelEvents.Instance.ColorGunHit.Invoke(color, shootPoint, radius);
    }

    private void DrawTrajectory()
    {
        Vector3 startPos = transform.position + m_mouseAiming.ThrowVector;
        RaycastHit2D raycastHit = Physics2D.Raycast(startPos, m_mouseAiming.CurrentDirection, Mathf.Infinity, mask);

        startPos.z = 1;
        Vector3 endPos = raycastHit.point;
        endPos.z = 1;
        m_lineRenderer.SetPositions(new Vector3[] { startPos, endPos });
        var color = ServiceLocator.LevelColorController.GetTileColorRGB(ServiceLocator.LevelColorController.CurrentColor);
        m_lineRenderer.startColor = color;
        m_lineRenderer.endColor = color;

        m_raycastHitPoint = raycastHit.point; 
    }

    private Vector3 m_raycastHitPoint;
    private LineRenderer m_lineRenderer;
    private MouseAiming m_mouseAiming;
}
