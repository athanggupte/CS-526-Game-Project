using System.Collections;
using UnityEngine;

public class ColorBlindEffector : MonoBehaviour
{
    public float effectTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
        LevelEvents.Instance.ColorBlindBegin.Invoke();
    }

    void Deactivate()
    {
        LevelEvents.Instance.ColorBlindEnd.Invoke();
    }

}
