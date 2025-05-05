using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    private Animator anim;

    private bool wasGroundedLastFrame = false;
    [SerializeField] private AudioClip footstepClip; // 🔈 Ayak sesi

    private Rigidbody2D rb;
    private AudioSource audioSource;

    public int facingDir { get; private set; } = 1;
    private float xInput;
    private bool facingRight = true;
    private bool isGrounded = false;

    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // 🔊 Ses kaynağını al
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        isGrounded = IsGroundDetected();
        anim.SetFloat("yVelocity", rb.velocity.y);
        rb.velocity = new Vector2(xInput * speed, rb.velocity.y);
        FlipController(xInput);
        // Yalnızca havadaysa zıplama animasyonu oynasın
        anim.SetBool("isJumping", !isGrounded);

        // Ayak sesi
        if (Mathf.Abs(xInput) > 0.1f && isGrounded && !audioSource.isPlaying)
        {
            audioSource.clip = footstepClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (audioSource.isPlaying && (!isGrounded && wasGroundedLastFrame || Mathf.Abs(xInput) < 0.1f))
        {
            Invoke(nameof(StopFootstep), 0.3f);
        }

        wasGroundedLastFrame = isGrounded;

        // Zıplama
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool("isJumping", true); // burada true yapılıyor
        }

        if (isGrounded)
        {
            anim.SetBool("isJumping", false); // yere değdiğinde tekrar false
        }

        anim.SetBool("isRunning", xInput != 0);
    }


    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight) Flip();
        else if (_x < 0 && facingRight) Flip();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Sahneyi yeniden yükle
        }
    }

    public virtual bool IsGroundDetected() =>
        Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public virtual bool IsWallDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance * facingDir);
    }
    private void StopFootstep()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();
    }
}