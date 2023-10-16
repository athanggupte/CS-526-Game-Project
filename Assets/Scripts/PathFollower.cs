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

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(waypoints);
        Assert.IsTrue(waypoints.Length > 0);
        currentIndex = 0;
        transform.position = waypoints[currentIndex].transform.position;

        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        Vector3 smoothedPosition = transform.position;

        while (true)
        {
            float t = 0;
            float totalDistance = Vector3.Distance(transform.position, waypoints[NextIndex].transform.position);

            while (t < 1)
            {
                Vector3 a = waypoints[currentIndex].transform.position;
                Vector3 b = waypoints[NextIndex].transform.position;
                Vector3 pos = Vector3.Lerp(a, b, t);

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
            currentIndex = NextIndex;
        }
    }

    int NextIndex => (currentIndex + 1) % waypoints.Length;

    private int currentIndex;
}
