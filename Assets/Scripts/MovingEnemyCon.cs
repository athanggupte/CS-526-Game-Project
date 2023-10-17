using UnityEngine;

public class MovingEnemyCon : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 dir = Vector3.left;
    private float speed = 2.0f;
    private bool check;
    private float curr;
    public float left_dist = 1;
    public float right_dist = 2;
    void Start()
    {
        check = true;
        curr = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * dir);
        if (check)
        {
            dir = Vector3.left;
            if (transform.position.x <= curr - left_dist)
            {
                check = false;
                dir = Vector3.right;
            }
        }
        else
        {
            dir = Vector3.right;
            if (transform.position.x >= curr + right_dist)
            {
                check = true;
                dir = Vector3.left;
            }

        }


    }
}
