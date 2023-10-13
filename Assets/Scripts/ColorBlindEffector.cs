using System.Collections;
using UnityEngine;

public class ColorBlindEffector : MonoBehaviour
{
    public float effectTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!m_active && collision.CompareTag("Player"))
        {
            StartCoroutine(ColorBlind());
        }
    }

    IEnumerator ColorBlind()
    {
        Activate();
        yield return new WaitForSeconds(effectTime);
        Deactivate();
    }

    void Activate()
    {
        m_active = true;
        LevelEvents.Instance.ColorBlindBegin.Invoke();
    }

    void Deactivate()
    {
        LevelEvents.Instance.ColorBlindEnd.Invoke();
        m_active = false;
    }

    private bool m_active = false;

}
