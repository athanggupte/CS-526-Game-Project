using UnityEngine;
using UnityEngine.Events;

public class ActiveZoneController : MonoBehaviour
{
    private string activeZoneName = "";
    private ZoneController activeZone;

    public Events.ActiveZoneChanged ActiveZoneChanged = new Events.ActiveZoneChanged();

    public string ActiveZoneName
    {
        get => activeZoneName;
    }

    public ZoneController ActiveZone
    {
        get => activeZone;
        set 
        {
            activeZone = value;
            activeZoneName = value == null ? "" : value.zoneName;
            ActiveZoneChanged.Invoke(activeZone);
        }
    }

    void Awake()
    {
        ServiceLocator.ActiveZoneController = this;
    }
}

namespace Events
{
    public class ActiveZoneChanged : UnityEvent<ZoneController>
    {
    }
}
