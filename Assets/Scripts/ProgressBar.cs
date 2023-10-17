using System;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    private float progress;

    void Awake()
    {
        progressIndicators = new GameObject[transform.childCount - 1];

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            progressIndicators[i] = transform.GetChild(i).gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetProgress(progress);
    }

    public void SetProgress(float progress)
    {
        this.progress = progress;
        int numIndicators = (int)(progress * progressIndicators.Length);
        numIndicators = Math.Min(numIndicators, progressIndicators.Length);

        for (int i = 0; i < numIndicators; i++)
        {
            progressIndicators[i].SetActive(true);
        }

        for (int i = numIndicators; i < progressIndicators.Length; i++)
        {
            progressIndicators[i].SetActive(false);
        }
    }

    private GameObject[] progressIndicators;

}
