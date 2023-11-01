using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullObjectMarker : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.85f, 0.23f, 0.67f, 0.8f);
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
