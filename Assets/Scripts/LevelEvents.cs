using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LevelEvents : MonoBehaviour
{
    public Events.ColorSwitchEvent ColorSwitchEvent = new Events.ColorSwitchEvent();
    public Events.ColorBombEvent ColorBombEvent = new Events.ColorBombEvent();
    public Events.ColorBlindBegin ColorBlindBegin = new Events.ColorBlindBegin();
    public Events.ColorBlindEnd ColorBlindEnd = new Events.ColorBlindEnd();

    public static LevelEvents Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        Instance = null;
    }
}

namespace Events
{
    public class ColorSwitchEvent : UnityEvent<LevelColor>
    {
    }

    /**
     * <summary>
     * Signals that a color bomb has detonated. The event handler should accept the 
     * parameters (LevelColor target_color, Vector3 position, float radius)
     * </summary>
     */
    public class ColorBombEvent : UnityEvent<LevelColor, Vector3, float>
    {
    }

    public class ColorBlindBegin : UnityEvent
    {
    }

    public class ColorBlindEnd : UnityEvent
    {
    }
}
