using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointData
{
    public static int CurrentCheckpointId = -1;
}

public class CheckpointController : MonoBehaviour
{
    [SerializeField] private ActiveZoneController activeZoneController;

    private Checkpoint m_currentCheckpoint;
    public Checkpoint CurrentCheckpoint
    {
        get => m_currentCheckpoint;
    }

    private Dictionary<ZoneController, Checkpoint> m_zoneCheckpointMap;

    void Awake()
    {
        m_zoneCheckpointMap = new Dictionary<ZoneController, Checkpoint>();
        activeZoneController.ActiveZoneChanged.AddListener(OnZoneChanged);
    }

    void Start()
    {
        foreach (Checkpoint checkpoint in FindObjectsByType<Checkpoint>(FindObjectsSortMode.None))
        {
            m_zoneCheckpointMap.Add(checkpoint.Zone, checkpoint);

            if (checkpoint.Id == CheckpointData.CurrentCheckpointId)
            {
                m_currentCheckpoint = checkpoint;
            }
        }

        if (m_currentCheckpoint == null)
        {
            SetCheckpoint(activeZoneController.ActiveZone);
        }

        Debug.Log(CheckpointData.CurrentCheckpointId);

        if (m_currentCheckpoint)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = m_currentCheckpoint.gameObject.transform.position;
        }

        CheckpointData.CurrentCheckpointId = -1;
    }

    private void OnZoneChanged(ZoneController zone)
    {
        SetCheckpoint(zone);
    }

    void SetCheckpoint(ZoneController zone)
    {
        Checkpoint newCheckpoint;
        if (zone != null && m_zoneCheckpointMap.TryGetValue(zone, out newCheckpoint))
        {
            // if (m_currentCheckpoint == null || newCheckpoint.Id > m_currentCheckpoint.Id)
            {
                m_currentCheckpoint = newCheckpoint;
            }
        }
    }
}
