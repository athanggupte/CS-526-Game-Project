using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LevelEvents : MonoBehaviour
{
    public static UnityEvent LevelEventsInitialized = new UnityEvent();


    public Events.ColorSwitch ColorSwitch = new Events.ColorSwitch();
    public Events.ColorBombDetonate ColorBombDetonate = new Events.ColorBombDetonate();
    public Events.ColorBlindBegin ColorBlindBegin = new Events.ColorBlindBegin();
    public Events.ColorBlindEnd ColorBlindEnd = new Events.ColorBlindEnd();
    public Events.StarCollect StarCollect = new Events.StarCollect();
    public Events.LevelEnd LevelEnd = new Events.LevelEnd();
    public Events.CollectBomb CollectBomb = new Events.CollectBomb();
    public Events.ColorSwitch OrbColorSwitch = new Events.ColorSwitch();
    public Events.BombEnemyDetonate BombEnemyDetonate = new Events.BombEnemyDetonate();

    public static LevelEvents Instance
    { 
        get => s_instance;
        private set
        {
            if (s_instance == value) return;
            if (s_instance != null)
            {
                LevelEventsInitialized.RemoveAllListeners();
            }

            s_instance = value;
        }
    }

    private static LevelEvents s_instance;

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
    public class ColorSwitch : UnityEvent<LevelColor>
    {
    }

    /**
     * <summary>
     * Signals that a color bomb has detonated. The event handler should accept the 
     * parameters (LevelColor target_color, Vector3 position, float radius)
     * </summary>
     */
    public class ColorBombDetonate : UnityEvent<LevelColor, Vector3, float>
    {
    }

    public class ColorBlindBegin : UnityEvent
    {
    }

    public class ColorBlindEnd : UnityEvent
    {
    }

    public class StarCollect : UnityEvent
    {
    }

    public class BombCollect : UnityEvent<LevelColor>
    {
    }

    public class LevelEnd : UnityEvent
    {
    }

    public class CollectBomb: UnityEvent
    {
    }

    public class BombEnemyDetonate : UnityEvent
    {
    }
}
