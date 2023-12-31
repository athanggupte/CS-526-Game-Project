using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PathFollower : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jitterFrequency;
    [SerializeField] private float jitterAmplitude;
    [SerializeField] private float smoothingFactor;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private bool AwakeOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(waypoints);
        Assert.IsTrue(waypoints.Length > 0);
        currentIndex = 0;
        transform.position = waypoints[currentIndex].transform.position;

        GetComponent<Spooker>().SpookedStart.AddListener(OnSpookedStart);
        GetComponent<Spooker>().SpookedEnd.AddListener(OnSpookedEnd);

        if (AwakeOnStart)
        {
            StartCoroutine(ExecutePatrol());
        }
    }

    private void OnSpookedStart()
    {
        speed *= 0.2f;
    }

    private void OnSpookedEnd()
    {
        speed /= 0.2f;
    }

    public IEnumerator FollowPath(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 smoothedPosition = transform.position;
        float t = 0;
        float totalDistance = Vector3.Distance(transform.position, endPosition);

        while (t < 1)
        {
            Vector3 pos = Vector3.Lerp(startPosition, endPosition, t);

            float jitterX = Mathf.PerlinNoise(t * jitterFrequency, Time.time) * jitterAmplitude;
            float jitterY = Mathf.PerlinNoise(t * jitterFrequency + 10, Time.time) * jitterAmplitude;

            pos.x += jitterX;
            pos.y += jitterY;

            smoothedPosition = Vector3.Lerp(smoothedPosition, pos, smoothingFactor);
            pos = smoothedPosition;

            transform.position = pos;

            t += speed / totalDistance * Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator ExecutePatrol(int startIndex = 0)
    {
        currentIndex = startIndex;
        while (true)
        {
            yield return FollowPath(waypoints[currentIndex].transform.position, waypoints[NextIndex].transform.position);
            currentIndex = NextIndex;
        }
    }

    int NextIndex => (currentIndex + 1) % waypoints.Length;

    private int currentIndex;
}
