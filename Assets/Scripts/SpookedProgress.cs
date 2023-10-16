using UnityEngine;

public class SpookedProgress : MonoBehaviour
{
    [SerializeField] private ProgressBar progressBar;

    void Start()
    {
        m_spooker = GetComponent<Spooker>();
    }

    void Update()
    {
        progressBar.gameObject.SetActive(m_spooker.SpookLevel > 0);
        progressBar.SetProgress(m_spooker.SpookLevel);
    }

    private Spooker m_spooker;
}
