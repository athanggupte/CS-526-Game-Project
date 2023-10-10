using UnityEngine;

public class Orb : MonoBehaviour
{
    public string orbID;

    private void Awake()
    {
        orbID = System.Guid.NewGuid().ToString();
    }
}