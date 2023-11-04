using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ColorGun : MonoBehaviour
{
    public LayerMask mask;
    [SerializeField] private BombFlash bombFlashPrefab;
    [SerializeField] private float radius;
    [SerializeField] private MouseAiming mouseAiming;

    private void Awake()
    {
        LevelEvents.LevelEventsInitialized.AddListener(ListenToLevelEvents);
    }

    void ListenToLevelEvents()
    {
        LevelEvents.Instance.StarActivate.AddListener(ActivateStar);
        LevelEvents.Instance.StarDeactivate.AddListener(DeactivateStar);
    }

    void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        m_lineRenderer.enabled = false;
    }

    void Update()
    {
        if (m_enabled)
        {
            Vector3 startPos = transform.position + mouseAiming.ThrowVector;
            RaycastHit2D raycastHit = Physics2D.Raycast(startPos, mouseAiming.CurrentDirection, Mathf.Infinity, mask);

            startPos.z = 1;
            Vector3 endPos = raycastHit.point;
            endPos.z = 1;
            m_lineRenderer.SetPositions(new Vector3[] { startPos, endPos });
            var color = ServiceLocator.LevelColorController.GetTileColorRGB(ServiceLocator.LevelColorController.CurrentColor);
            m_lineRenderer.startColor = color;
            m_lineRenderer.endColor = color;

            m_raycastHitPoint = raycastHit.point;
        }
    }


    public void Shoot()
    {
        var color = ServiceLocator.LevelColorController.CurrentColor;

        var bombFlash = Instantiate(bombFlashPrefab);
        bombFlash.transform.SetPositionAndRotation(m_raycastHitPoint, Quaternion.identity);
        bombFlash.transform.localScale = Vector3.one * (0.5f + radius * 2);
        bombFlash.color = color;

        LevelEvents.Instance.ColorBombDetonate.Invoke(color, m_raycastHitPoint, radius);
    }

    void ActivateStar()
    {
        m_enabled = true;
        m_lineRenderer.enabled = true;
        Debug.Log("Star Activated!");
    }

    void DeactivateStar()
    {
        m_enabled = false;
        m_lineRenderer.enabled = false;
        Debug.Log("Star Deactivated!");
    }

    private Vector3 m_raycastHitPoint;
    private LineRenderer m_lineRenderer;
    private bool m_enabled;
}
