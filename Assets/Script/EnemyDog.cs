using UnityEngine;


public class EnemyDog : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = 0.1f;
    private Animator anim;



    private Transform player;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private int facingDir = 1;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float verticalDistance = Mathf.Abs(player.position.y - transform.position.y);
        float horizontalDistance = Mathf.Abs(player.position.x - transform.position.x);
        bool onGround = IsGroundDetected();
        bool hitWall = IsWallDetected();
        bool playerOnRight = player.position.x > transform.position.x;
        int moveDir = playerOnRight ? 1 : -1;

        bool seesPlayer = horizontalDistance < chaseRange && verticalDistance < 2.5f;

        // 🎯 Oyuncuyu görüyorsa hız 3.5, görmüyorsa 2
        speed = seesPlayer ? 3.5f : 2f;

        bool shouldRun = (seesPlayer && onGround && !hitWall && IsGroundAhead())
              || (onGround && !hitWall && IsGroundAhead());

        anim.SetBool("isRunning", shouldRun);

        if (seesPlayer && onGround)
        {
            if (moveDir != facingDir)
                Flip();

            if (!hitWall && IsGroundAhead())
                rb.velocity = new Vector2(speed * facingDir, rb.velocity.y);
            else
                rb.velocity = Vector2.zero;
        }
        else
        {
            if (onGround && !hitWall && IsGroundAhead())
            {
                rb.velocity = new Vector2(speed * facingDir, rb.velocity.y);
            }
            else
            {
                rb.velocity = Vector2.zero;
                Flip();
            }
        }
    }





    private void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private bool IsGroundDetected()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private bool IsWallDetected()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);

        if (wallCheck != null)
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance * facingDir);
    }
    private bool IsGroundAhead()
    {
        Vector2 direction = Vector2.right * facingDir;
        Vector2 origin = groundCheck.position + (Vector3)direction * 0.2f;

        return Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, whatIsGround);
    }

}