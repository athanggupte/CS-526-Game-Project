using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerColorAdapter : MonoBehaviour
{
    void Awake()
    {
        LevelEvents.LevelEventsInitialized.AddListener(ListenToLevelEvents);
    }

    void ListenToLevelEvents()
    {
        LevelEvents.Instance.ColorSwitch.AddListener(OnColorSwitch);
    }

    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnColorSwitch(LevelColor color, string zoneName)
    {
        if (!m_spriteRenderer) m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.color = ServiceLocator.LevelColorController.GetTileColorRGB(color);
    }

    private SpriteRenderer m_spriteRenderer;
}
