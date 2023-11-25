using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private bool isFacingRight = true;
    
    private Vector2 currentVelocity = Vector2.zero;

    public float speed = 8f;
    public float acceleration = 20f;
    public float inAirSpeed = 6f;
    public float jumpHeightMin = 1.5f;
    public float jumpHeightMax = 4.5f;
    public float groundCheckRadius = 0.2f;
    
    private bool grounded;
    private bool wantsMove;
    private bool jumpPressed;
    private bool jumpHeld;
    private float jumpTimeMin = 0.4f;
    private float jumpTimeMax = 1f;
    private float jumpSpeed = 8f;
    private float jumpTimer = 0;
    private float coyoteTime = 0.5f;
    private float coyoteTimer = 0;

    private Rigidbody2D rb;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    enum State
    {
        Jumping,
        Falling,
        Grounded
    }

    State state = State.Grounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float g = Mathf.Abs(rb.gravityScale * Physics2D.gravity.y);
        // float t = jumpTimeMin;
        // float h = jumpHeightMin;
        // float u = jumpSpeed;
        // jumpingPower = Mathf.Sqrt(g*g * t*t + 2*g*h) - g*t;

        jumpTimeMin = jumpHeightMin / jumpSpeed - jumpSpeed / (2 * g);
        jumpTimeMax = jumpHeightMax / jumpSpeed - jumpSpeed / (2 * g);

        Assert.IsTrue(jumpTimeMin > 0);
        Assert.IsTrue(jumpTimeMax > jumpTimeMin);
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        wantsMove = (horizontal != 0f);

        jumpPressed = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButton("Jump");

        Flip();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(200,200,100,100), state.ToString());
        GUI.Label(new Rect(200,220,100,100), "Jump : " + (jumpPressed || jumpHeld).ToString());
        GUI.Label(new Rect(200,240,100,100), "ground check : " + grounded.ToString());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        // Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(2 * groundCheckRadius, 2 * groundCheckRadius, 2 * groundCheckRadius));
    }

    private void FixedUpdate()
    {
        grounded = (bool)Physics2D.OverlapBox(groundCheck.position, new Vector2(2 * groundCheckRadius, 2 * groundCheckRadius), 0, groundLayer);

        if (!grounded && state == State.Grounded)
        {
            state = State.Falling;
            coyoteTime = 0f;
        }

        switch (state)
        {
            case State.Jumping:
                Jumping();
                break;
            case State.Falling:
                Falling();
                break;
            case State.Grounded:
                Grounded();
                break;
        }

        rb.velocity = currentVelocity;
    }

    private void Jumping()
    {
        jumpTimer += Time.fixedDeltaTime;
        //Debug.Log("fixedDeltaTime : " + Time.fixedDeltaTime);

        if (wantsMove)
        {
            currentVelocity.x = horizontal * inAirSpeed;
        }
        else
        {
            currentVelocity.x *= 0.65f;
        }

        currentVelocity.y = rb.velocity.y;
        if (jumpTimer < jumpTimeMin || (jumpHeld && jumpTimer < jumpTimeMax))
        {
            // Debug.Log("Applied jump velocity");
            // Debug.Log("jumpTimer : " + jumpTimer + " | fixedDeltaTime : " + Time.fixedDeltaTime);
            currentVelocity.y = jumpSpeed;
        }

        if (rb.velocity.y < 0f)
        {
            state = State.Falling;
        }
    }

    private void Falling()
    {
        if (wantsMove)
        {
            currentVelocity.x = horizontal * inAirSpeed;
        }
        else
        {
            currentVelocity.x *= 0.65f;
        }

        currentVelocity.y = rb.velocity.y;

        if (grounded)
        {
            state = State.Grounded;
        }
    }

    private void Grounded()
    {
        if (wantsMove)
        {
            currentVelocity.x += horizontal * acceleration * Time.fixedDeltaTime;
            currentVelocity.x = Mathf.Clamp(currentVelocity.x, -speed, speed);
        }
        else
        {
            currentVelocity.x *= 0.65f;
        }

        if (jumpPressed || jumpHeld)
        {
            //currentVelocity.y = jumpingPower;
            state = State.Jumping;
            jumpTimer = 0;
        }
    }

    public bool IsGrounded()
    {
        return grounded;
    }

    public Transform GroundCheckTransform()
    {
        return groundCheck.transform;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}