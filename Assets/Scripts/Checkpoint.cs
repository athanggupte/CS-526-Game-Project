using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private ZoneController zone;

    public int Id { get => id; }
    public ZoneController Zone { get => zone; }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawIcon(transform.position, "checkpoint1.png");
    }
}
