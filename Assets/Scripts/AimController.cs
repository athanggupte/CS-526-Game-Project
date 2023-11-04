using TMPro;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] private GameObject contextualHintTextPrefab;
    [SerializeField] private ColorGun colorGun;

    // Start is called before the first frame update
    void Start()
    {
        m_mouseAiming = GetComponent<MouseAiming>();
        m_weaponController = GetComponent<PlayerWeaponController>();
    }

    void Update()
    {
        if (!ServiceLocator.LevelColorController.IsStarActivated)
        {
            m_mouseAiming.ShowReticle = true;
            m_mouseAiming.ShowGun = false;

            if (Input.GetMouseButtonDown(0 /* LMB */))
            {


                //if (!m_bombThrower.HasBomb())
                //{
                //    if (m_lastNoBombsCollectedTooltip == null)
                //    {
                //        m_numClicksBeforeCollectBombMessage -= 1;
                //        if (m_numClicksBeforeCollectBombMessage == 0)
                //        {
                //            m_numClicksBeforeCollectBombMessage = 3;
                //            GameObject textGo = Instantiate(contextualHintTextPrefab);
                //            textGo.GetComponent<TextMeshPro>().text = "No bombs collected";
                //            ContextualTooltip tooltip = textGo.GetComponent<ContextualTooltip>();
                //            tooltip.StickToTarget(gameObject, new Vector3(0, 3, 2));
                //            tooltip.Deploy(2.5f);

                //            m_lastNoBombsCollectedTooltip = textGo;
                //        }
                //    }
                //}
                //else if (m_bombThrower.IsLastBombActive)
                //{
                //    m_bombThrower.DetonateBomb();
                //}
                //else
                //{
                //    m_bombThrower.ThrowBomb(m_mouseAiming.ThrowVector);
                //}
            }
        }
        else
        {
            m_mouseAiming.ShowReticle = false;
            m_mouseAiming.ShowGun = true;

            if (Input.GetMouseButtonDown(0 /* LMB */))
            {
                colorGun.Shoot();
            }
        }
    }

    private MouseAiming m_mouseAiming;
    private PlayerWeaponController m_weaponController;

    private int m_numClicksBeforeCollectBombMessage = 3;
    private GameObject m_lastNoBombsCollectedTooltip;
}
