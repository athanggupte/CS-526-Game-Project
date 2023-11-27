using TMPro;
using UnityEngine;

public class CollectibleBomb : MonoBehaviour
{
    [SerializeField] private LevelColor color;
    [SerializeField] private GameObject tooltipPrefab;

    void Start()
    {
        m_weaponController = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponController>();
    }

    public void SetColor(LevelColor color)
    {
        this.color = color;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = ServiceLocator.LevelColorController.GetTileColorRGB(color);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (m_weaponController.BombHandler.CurrentBombColor != color || m_weaponController.BombHandler.AmmoCount < WeaponController.MAX_BOMB_AMMO)
            {
                LevelEvents.Instance.BombCollect.Invoke(color, ServiceLocator.ActiveZoneController.ActiveZoneName);
                Destroy(gameObject);
            }
            else
            {
                GameObject textGo = Instantiate(tooltipPrefab);
                textGo.GetComponent<TextMeshPro>().text = "Ammo full";
                ContextualTooltip tooltip = textGo.GetComponent<ContextualTooltip>();
                GameObject player = m_weaponController.gameObject;
                tooltip.StickToTarget(player, new Vector3(0, 3, 2));
                tooltip.Deploy(5.0f);
            }
        }
    }

    private WeaponController m_weaponController;
}
