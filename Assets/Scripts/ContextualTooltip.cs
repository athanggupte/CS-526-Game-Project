using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextualTooltip : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Vector3 offset;

    public void StickToTarget(GameObject targetObject, Vector3 offset)
    {
        this.targetObject = targetObject;
        this.offset = offset;
    }

    public void Deploy(float secondsAlive)
    {
        StartCoroutine(DestroySelf(secondsAlive));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = targetObject.transform.position + offset;
    }

    IEnumerator DestroySelf(float secondsAlive)
    {
        yield return new WaitForSeconds(secondsAlive);
        Destroy(gameObject);
    }

}
