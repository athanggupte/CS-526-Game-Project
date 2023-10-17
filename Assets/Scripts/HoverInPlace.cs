using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverInPlace : MonoBehaviour
{
    public float frequency;
    public float amplitude;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y += Mathf.Sin(frequency * Time.time) * amplitude * frequency * Time.deltaTime;
        transform.position = pos;
    }
}
