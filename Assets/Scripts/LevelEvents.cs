using UnityEngine;
using UnityEngine.Events;

public enum LevelEndCondition
{
    GoalReached,
    LevelRestarted,
    GameQuit
}

[System.Serializable]
public class LevelEvents : MonoBehaviour
{
    public static UnityEvent LevelEventsInitialized = new UnityEvent();


    public Events.ColorSwitch ColorSwitch = new Events.ColorSwitch();
    public Events.ColorBombDetonate ColorBombDetonate = new Events.ColorBombDetonate();
    public Events.ColorGunHit ColorGunHit = new Events.ColorGunHit();
    public Events.ColorBlindBegin ColorBlindBegin = new Events.ColorBlindBegin();
    public Events.ColorBlindEnd ColorBlindEnd = new Events.ColorBlindEnd();
    public Events.StarCollect StarCollect = new Events.StarCollect();
    public Events.LevelEnd LevelEnd = new Events.LevelEnd();
    public Events.BombCollect BombCollect = new Events.BombCollect();
    public Events.GunCollect GunCollect = new Events.GunCollect();
    public Events.ColorSwitch OrbColorSwitch = new Events.ColorSwitch();
    public Events.BombEnemyDetonate BombEnemyDetonate = new Events.BombEnemyDetonate();
    public Events.StarActivate StarActivate = new Events.StarActivate();
    public Events.StarDeactivate StarDeactivate = new Events.StarDeactivate();
    public Events.NoAmmoBomb NoAmmoBomb = new Events.NoAmmoBomb();
    public Events.NoAmmoGun NoAmmoGun = new Events.NoAmmoGun();
    public Events.ColorBombThrow ColorBombThrow = new Events.ColorBombThrow();

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
    public class ColorSwitch : UnityEvent<LevelColor, string>
    {
    }

    /**
     * <summary>
     * Signals that a color bomb has detonated. The event handler should accept the 
     * parameters (LevelColor target_color, Vector3 position, float radius)
     * </summary>
     */
    public class ColorBombDetonate : UnityEvent<LevelColor, Vector3, float, string>
    {
    }

    public class ColorGunHit : UnityEvent<LevelColor, Vector3, float, string>
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

    public class BombCollect : UnityEvent<LevelColor, string>
    {
    }

    public class GunCollect : UnityEvent<string>
    {
    }

    public class LevelEnd : UnityEvent<LevelEndCondition>
    {
    }

    public class BombEnemyDetonate : UnityEvent<int>
    {
    }

    public class StarActivate : UnityEvent
    {
    }

    public class StarDeactivate : UnityEvent
    {
    }

    public class NoAmmoGun : UnityEvent
    {
    }

    public class NoAmmoBomb : UnityEvent
    {
    }

    public class ColorBombThrow: UnityEvent<string>
    {
    }

}
