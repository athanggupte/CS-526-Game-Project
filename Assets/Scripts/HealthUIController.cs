using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    [SerializeField] Sprite HeartSprite;
    [SerializeField] Sprite HalfHeartSprite;

    private int NUM_HEARTS = 4;

    // Start is called before the first frame update
    void Start()
    {
        m_healthController = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>();

        m_healthIndicators = new GameObject[NUM_HEARTS];
        m_healthIndicatorImages = new Image[NUM_HEARTS];
        for (int i = 0; i < NUM_HEARTS;  i++)
        {
            m_healthIndicators[i] = transform.GetChild(i).gameObject;
            m_healthIndicatorImages[i] = m_healthIndicators[i].GetComponent<Image>();

            Assert.IsNotNull(m_healthIndicators[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < m_healthController.Health; i += 2)
        {
            m_healthIndicators[i / 2].SetActive(true);
        }
        for (int i = m_healthController.Health + (m_healthController.Health % 2); i < NUM_HEARTS; i += 2)
        {
            m_healthIndicators[i / 2].SetActive(false);
        }
    }

    private HealthController m_healthController;
    private GameObject[] m_healthIndicators;
    private Image[] m_healthIndicatorImages;
}
