using UnityEngine;

public class GoalTriangleAnimation : MonoBehaviour
{
    float maxDisplacement = 0.2f;
    float speed = 2.5f;

    float baseY;

    // Start is called before the first frame update
    void Start()
    {
        baseY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        var tmp = transform.position;
        tmp.y = baseY + Mathf.Sin(Time.time * speed) * maxDisplacement;

        transform.position = tmp;
    }
}
