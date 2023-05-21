using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float airControl;
    public float jumpHeight;
    public float walljumpForce;
    public int wallJumps;
    public float gravity;

    public LayerMask groundMask;
    public LayerMask groundMovingMask;

    [Header("Items")]
    public Transform[] groundCheck;
    public Transform leftCheck;
    public Transform rightCheck;

    private float xyMovement;
    private int wjs;
    private float grabWait;
    private float activeSpeed;
    private Vector2 velocity;

    public bool grounded;
    public bool groundMoving;
    public bool grabbingWall;
    public int wallCollision;
    public bool touchingObject;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravity;

        wjs = wallJumps;
    }

    // Update is called once per frame
    void Update()
    {
        xyMovement = Input.GetAxisRaw("Horizontal");

        if (!grounded && !groundMoving)
        {
            activeSpeed = speed * 1.2f;
        }
        else
        {
            activeSpeed = speed;
        }

        if (!grabbingWall)
            rb.position += new Vector2(xyMovement * activeSpeed, 0) * Time.deltaTime;
        UpdateCollisionDetection();

        if (grounded || groundMoving)
        {
            wallJumps = wjs;
            velocity.x = 0;
        }

        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, 0.75f * Time.deltaTime), rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            float _jumpVelo = Mathf.Sqrt(jumpHeight * -2 * -gravity);
            velocity.y = _jumpVelo;
            rb.velocity = velocity;
        }
        else if (Input.GetKey(KeyCode.LeftControl) && touchingObject && Time.realtimeSinceStartup >= grabWait)
        {
            grabbingWall = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            grabbingWall = false;
        }
        if (grabbingWall)
        {
            velocity.y = 0;
            rb.gravityScale = 0;
            rb.velocity = velocity;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                grabbingWall = false;
                grabWait = Time.realtimeSinceStartup + 0.15f;
                rb.AddForce(Vector2.right * wallCollision * walljumpForce + Vector2.up * walljumpForce * (gravity / 11), ForceMode2D.Impulse);
            }
        }
        else
        {
            rb.gravityScale = gravity;

            if (xyMovement == -wallCollision)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }
    void UpdateCollisionDetection()
    {
        grounded = false;
        groundMoving = false;
        for (int i = 0; i < groundCheck.Length; i++)
        {
            if (!grounded)
                grounded = Physics2D.CircleCast(groundCheck[i].position, 0.05f, Vector2.down, 0.05f, groundMask);
            if (!groundMoving)
                groundMoving = Physics2D.CircleCast(groundCheck[i].position, 0.05f, Vector2.down, 0.05f, groundMovingMask);
        }

        if (Physics2D.CircleCast(leftCheck.position, 0.2f, Vector2.left, 0.2f, groundMask))
        {
            wallCollision = 1;
        }
        else if (Physics2D.CircleCast(rightCheck.position, 0.2f, Vector2.right, 0.2f, groundMask))
        {
            wallCollision = -1;
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        touchingObject = true;
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        touchingObject = false;
    }
}
