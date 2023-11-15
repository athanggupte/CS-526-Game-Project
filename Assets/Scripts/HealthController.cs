using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int health = 8;

    public int Health { get => health; }

    void Awake()
    {
        LevelEvents.LevelEventsInitialized.AddListener(ListenToLevelEvents);
    }

    private void ListenToLevelEvents()
    {
        LevelEvents.Instance.ColorBombDetonate.AddListener(ColorBombDetonated);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void ColorBombDetonated(LevelColor color, Vector3 position, float radius)
    {
        if (ServiceLocator.LevelColorController.CurrentColor != color)
        {
            if (Vector3.Distance(transform.position, position) < radius)
            {
                health -= 1;
            }
        }
    }

}
