using TMPro;
using UnityEngine;

public class AimedBombThrower : MonoBehaviour
{
    [SerializeField] private GameObject contextualHintTextPrefab;

    // Start is called before the first frame update
    void Start()
    {
        m_mouseAiming = GetComponent<MouseAiming>();
        m_bombThrower = GetComponent<BombThrower>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0 /* LMB */))
        {
            if (!m_bombThrower.HasBomb())
            {
                if (m_lastNoBombsCollectedTooltip == null)
                {
                    m_numClicksBeforeCollectBombMessage -= 1;
                    if (m_numClicksBeforeCollectBombMessage == 0)
                    {
                        m_numClicksBeforeCollectBombMessage = 5;
                        GameObject textGo = Instantiate(contextualHintTextPrefab);
                        textGo.GetComponent<TextMeshPro>().text = "No bombs collected";
                        ContextualTooltip tooltip = textGo.GetComponent<ContextualTooltip>();
                        tooltip.StickToTarget(gameObject, new Vector3(0, 3, 2));
                        tooltip.Deploy(5.0f);

                        m_lastNoBombsCollectedTooltip = textGo;
                    }
                }
            }
            else if (m_bombThrower.IsLastBombActive)
            {
                m_bombThrower.DetonateBomb();
            }
            else
            {
                m_bombThrower.ThrowBomb(m_mouseAiming.ThrowVector);
            }
        }
    }

    private MouseAiming m_mouseAiming;
    private BombThrower m_bombThrower;
    
    private int m_numClicksBeforeCollectBombMessage = 5;
    private GameObject m_lastNoBombsCollectedTooltip;
}
