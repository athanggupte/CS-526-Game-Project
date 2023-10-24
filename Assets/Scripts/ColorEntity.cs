using System;
using UnityEngine;
using UnityEngine.Assertions;

public class ColorEntity : MonoBehaviour
{
    public LevelColor color;
    public Sprite inactiveSprite;

    private Sprite activeSprite;

    // Start is called before the first frame update
    void Awake()
    {
        LevelEvents.LevelEventsInitialized.AddListener(ListenToLevelEvents);
    }

    private void ListenToLevelEvents()
    {
        LevelEvents.Instance.ColorSwitch.AddListener(SwitchColor);
        LevelEvents.Instance.ColorBombDetonate.AddListener(ColorBomb);
        LevelEvents.Instance.ColorBlindBegin.AddListener(BeginColorBlind);
        LevelEvents.Instance.ColorBlindEnd.AddListener(EndColorBlind);

        activeSprite = GetComponent<SpriteRenderer>().sprite;
    }

    void SwitchColor(LevelColor levelColor)
    {
        m_active = (levelColor == color);

        if (!m_colorBlinded)
        {
            Color tmpColor = GetComponent<SpriteRenderer>().color;
            tmpColor.a = (m_active) ? 1.0f : 0.2f;
            GetComponent<SpriteRenderer>().color = tmpColor;
        }

        GetComponent<SpriteRenderer>().sprite = (m_active) ? activeSprite : inactiveSprite;

        GetComponent<Collider2D>().enabled = m_active;
    }
    void ColorBomb(LevelColor targetColor, Vector3 position, float radius)
    {
        if (m_active)
        {
            var diffPositions = transform.position - position;
            if (Mathf.Abs(diffPositions.x) < (radius + 0.5) && Mathf.Abs(diffPositions.y) < (radius + 0.5))
            {
                color = targetColor;
                GetComponent<SpriteRenderer>().color = ServiceLocator.LevelColorController.GetTileColorRGB(color);
                SwitchColor(ServiceLocator.LevelColorController.CurrentColor);
            }
        }
    }

    void BeginColorBlind()
    {
        m_colorBlinded = true;
        GetComponent<SpriteRenderer>().color = ServiceLocator.LevelColorController.ColorBlindColorRGB;
    }

    void EndColorBlind()
    {
        Color tmpColor = ServiceLocator.LevelColorController.GetTileColorRGB(color);
        tmpColor.a = (m_active) ? 1.0f : 0.2f;
        GetComponent<SpriteRenderer>().color = tmpColor;

        m_colorBlinded = false;
    }

    public bool IsActive()
    {
        return m_active;
    }

    
    private bool m_active;
    private bool m_colorBlinded;
}
