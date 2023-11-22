using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveZoneController : MonoBehaviour
{
    public string activeZoneName = "";

    void Awake()
    {
        ServiceLocator.ActiveZoneController = this;
    }
}
