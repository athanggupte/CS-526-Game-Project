using System.Collections;
using TMPro;
using UnityEngine;

public class ColorBombEffector : MonoBehaviour
{
    public LevelColor color;
    public int radius;
    public float timeToExplode;
    public BombFlash bombFlashPrefab;

    public TextMeshPro timerText;

    public void Deploy()
    {
        GetComponentInChildren<SpriteRenderer>().color = ServiceLocator.LevelColorController.GetTileColorRGB(color);

        m_countDownTimerCoroutine = StartCoroutine(CountDownTimer());
        m_timedDetonateCoroutine = StartCoroutine(TimedDetonate());
    }

    public void Detonate()
    {
        var bombFlash = Instantiate(bombFlashPrefab);
        bombFlash.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        bombFlash.transform.localScale = Vector3.one * (0.5f + radius * 2);
        bombFlash.color = color;

        LevelEvents.Instance.ColorBombDetonate.Invoke(color, transform.position, radius, ServiceLocator.ActiveZoneController.activeZoneName);
        Destroy(gameObject);
    }

    IEnumerator TimedDetonate()
    {
        yield return new WaitForSeconds(timeToExplode + 0.25f);
        Detonate();
        Destroy(gameObject);
    }

    IEnumerator CountDownTimer()
    {
        while (timeToExplode > 0)
        {
            if (timerText)
                timerText.text = ((int)timeToExplode).ToString();
            timeToExplode--;
            yield return new WaitForSeconds(0.99f);
        }
    }

    private Coroutine m_countDownTimerCoroutine;
    private Coroutine m_timedDetonateCoroutine;

}
