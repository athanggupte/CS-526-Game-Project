using System.Collections;
using TMPro;
using UnityEngine;

public class ColorBombEffector : MonoBehaviour
{
    public LevelColor color;
    public int radius;
    public LevelColorController levelColorController;
    public float timeToExplode;

    public TextMeshPro timerText;

    public void Deploy()
    {
        StartCoroutine(CountDownTimer());
        StartCoroutine(BlowUp());
    }

    IEnumerator BlowUp()
    {
        yield return new WaitForSeconds(timeToExplode + 0.25f);
        LevelEvents.Instance.ColorBombDetonate.Invoke(color, transform.position, radius);
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
}
