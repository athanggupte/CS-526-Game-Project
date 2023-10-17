using System.Collections;
using UnityEngine;

public class ColorBlindEffector : MonoBehaviour
{
    [SerializeField] private float effectTime;

    public void Deploy()
    {
        if (!m_active)
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
