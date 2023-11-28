using UnityEngine;

public static class ServiceLocator
{
    public static LevelColorController LevelColorController { get; set; }

    public static DataCollector DataCollector { get; set; }

    public static OrbController OrbController { get; set; }

    public static ActiveZoneController ActiveZoneController { get; set; }

    public static AudioSource AudioSource { get; set; }
}
