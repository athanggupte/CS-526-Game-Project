using UnityEngine;

public class OrbController : MonoBehaviour
{
    public string previousOrbID = "";

    void Awake()
    {
        ServiceLocator.OrbController = this;
    }
}
