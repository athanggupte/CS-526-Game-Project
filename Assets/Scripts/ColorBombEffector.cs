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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Deploy()
    {
        StartCoroutine(CountDownTimer());
        StartCoroutine(BlowUp());
    }

    void BlowUpTiles()
    {
        LevelEvents.Instance.ColorBombDetonate.Invoke(color, transform.position, radius);

        //foreach (var enemyGO in GameObject.FindGameObjectsWithTag("Enemy"))
        //{
        //    if (Mathf.Abs(enemyGO.transform.position.x - transform.position.x) < radius &&
        //        Mathf.Abs(enemyGO.transform.position.y - transform.position.y) < radius)
        //    {
        //        enemyGO.GetComponent<EnemyController>().color = color;
        //    }
        //}
    }

    IEnumerator BlowUp()
    {
        yield return new WaitForSeconds(timeToExplode + 0.25f);
        BlowUpTiles();
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
