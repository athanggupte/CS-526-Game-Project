using System;
using UnityEngine;
using UnityEngine.Assertions;

public class ColorEntity : MonoBehaviour
{
    public LevelColor color;

    // Start is called before the first frame update
    void Awake()
    {
        LevelEvents.LevelEventsInitialized.AddListener(ListenToLevelEvents);
    }

    private void ListenToLevelEvents()
    {
        LevelEvents.Instance.ColorSwitch.AddListener(SwitchColor);
        LevelEvents.Instance.ColorBombDetonate.AddListener(ColorBomb);
        LevelEvents.Instance.ColorGunHit.AddListener(ColorGun);
        LevelEvents.Instance.ColorBlindBegin.AddListener(BeginColorBlind);
        LevelEvents.Instance.ColorBlindEnd.AddListener(EndColorBlind);
    }

    void Start()
    {
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        m_layerMaskGround = LayerMask.NameToLayer("Ground Layer");
        m_layerMaskInactive = LayerMask.NameToLayer("Inactive");
    }

    void SwitchColor(LevelColor levelColor, string zoneName)
    {
        if (!m_spriteRenderer) m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        m_active = (levelColor == color);

        if (!m_colorBlinded)
        {
            Assert.IsNotNull(m_spriteRenderer);
            Color tmpColor = m_spriteRenderer.color;
            tmpColor.a = (m_active) ? 1.0f : 0.2f;
            m_spriteRenderer.color = tmpColor;
        }

        gameObject.layer = m_active ? m_layerMaskGround : m_layerMaskInactive;
    }

    public void ReapplyColor()
    {
        if (!m_spriteRenderer) m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (!m_colorBlinded)
        {
            Color tmpColor = ServiceLocator.LevelColorController.GetTileColorRGB(color);
            tmpColor.a = (m_active) ? 1.0f : 0.2f;
            m_spriteRenderer.color = tmpColor;
        }

        gameObject.layer = m_active ? m_layerMaskGround : m_layerMaskInactive;
    }

    void ColorBomb(LevelColor targetColor, Vector3 position, float radius, string zoneName)
    {
        if (m_active)
        {
            var diffPositions = transform.position - position;
            if (Mathf.Abs(diffPositions.x) < (radius + 0.5) && Mathf.Abs(diffPositions.y) < (radius + 0.5))
            {
                color = targetColor;
                m_spriteRenderer.color = ServiceLocator.LevelColorController.GetTileColorRGB(color);
                SwitchColor(ServiceLocator.LevelColorController.CurrentColor, zoneName);
            }
        }
    }

    void ColorGun(LevelColor targetColor, Vector3 position, float radius, string zoneName)
    {
        if (!m_active)
        {
            var diffPositions = transform.position - position;
            if (Mathf.Abs(diffPositions.x) < (radius + 0.5) && Mathf.Abs(diffPositions.y) < (radius + 0.5))
            {
                color = targetColor;
                m_spriteRenderer.color = ServiceLocator.LevelColorController.GetTileColorRGB(color);
                SwitchColor(ServiceLocator.LevelColorController.CurrentColor, ServiceLocator.ActiveZoneController.activeZoneName);
            }
        }
    }

    void BeginColorBlind()
    {
        m_colorBlinded = true;
        m_spriteRenderer.color = ServiceLocator.LevelColorController.ColorBlindColorRGB;
    }

    void EndColorBlind()
    {
        Color tmpColor = ServiceLocator.LevelColorController.GetTileColorRGB(color);
        tmpColor.a = (m_active) ? 1.0f : 0.2f;
        m_spriteRenderer.color = tmpColor;

        m_colorBlinded = false;
    }

    public bool IsActive()
    {
        return m_active;
    }

    
    private bool m_active;
    private bool m_colorBlinded;

    private SpriteRenderer m_spriteRenderer;

    int m_layerMaskGround;
    int m_layerMaskInactive;
}
