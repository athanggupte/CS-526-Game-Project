using UnityEngine;

public class MovingEnemyCon : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 dir = Vector3.right;
    private float speed = 2.0f;
    private bool check;
    private float curr;
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
            dir= Vector3.right;
            if (transform.position.x <= curr - 1)
            {
                check = false;
                dir = Vector3.left;
            }
        }
        else
        {
            dir = Vector3.left;
            if(transform.position.x>=curr+1)
            {
                check = true;
                dir = Vector3.right;
            }

        }


    }
}
