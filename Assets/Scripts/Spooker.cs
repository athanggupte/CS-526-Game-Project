using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Spooker : MonoBehaviour
{
    public UnityEvent SpookedEvent = new UnityEvent();

    [SerializeField] private float spookSpeed     = 1.6f; // Speed at which the NPC gets spooked
    [SerializeField] private float spookTimeout   = 5f;   // Timeout after spooked
    [SerializeField] private float unspookSpeed   = 0.4f; // Speed at which the NPC gets spooked
    [SerializeField] private float unspookTimeout = 1f;   // Timeout before unspooking starts

    public bool IsSpooked
    {
        get => m_spookLevel == 0;
    }

    public float SpookLevel
    {
        get => 1 - m_spookLevel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_spooking = true;
            StartCoroutine(Spooking());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_spooking = false;
            StartCoroutine(Unspooking());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_spookLevel = 1;
    }

    IEnumerator Spooking()
    {
        while (m_spooking)
        {
            if (m_spookLevel > 0)
            {
                m_spookLevel -= spookSpeed * Time.deltaTime;
                m_spookLevel = Mathf.Clamp01(m_spookLevel);
            }

            if (m_spookLevel == 0)
            {
                SpookedEvent.Invoke();
                yield return new WaitForSeconds(spookTimeout);
            }

            yield return null;
        }
    }

    IEnumerator Unspooking()
    {
        yield return new WaitForSeconds(unspookTimeout);

        while (!m_spooking && m_spookLevel < 1)
        {
            m_spookLevel += unspookSpeed * Time.deltaTime;
            m_spookLevel = Mathf.Clamp01(m_spookLevel);

            yield return null;
        }
    }

    private bool m_spooking;
    private float m_spookLevel;
}
