using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialColorBlinder : MonoBehaviour
{
    [SerializeField] private PathFollower pathFollower;
    [SerializeField] private GameObject[] waypoints;
    private bool interacted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SwoopIn()
    {
        Transform blinderTransform = pathFollower.gameObject.GetComponent<Transform>();
        
        yield return pathFollower.FollowPath(blinderTransform.position, waypoints[0].transform.position);
        yield return pathFollower.FollowPath(blinderTransform.position, waypoints[1].transform.position);

        StartCoroutine(pathFollower.ExecutePatrol(3));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !interacted)
        {
            interacted = true;
            StartCoroutine(SwoopIn());
        }
    }
}
