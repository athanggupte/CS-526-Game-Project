using System.Collections;
using UnityEngine;

public class ColorBlindEffector : MonoBehaviour
{
    [SerializeField] private float effectTime;
    private float totalColorBlindTime = 0f;  // Added variable


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
        totalColorBlindTime += effectTime;  // Update total time
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

    public float GetTotalColorBlindTime()
    {
        return totalColorBlindTime;
    }

    private bool m_active = false;

}
