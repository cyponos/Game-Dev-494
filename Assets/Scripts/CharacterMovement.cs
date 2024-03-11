using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public AudioSource AudioSource;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float directionX;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    // Start is called before the first frame update
    private void Start()
    {
        //grab references for rigidbody and animator
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        speed = 10;
        jumpPower = 12;
        directionX = Input.GetAxisRaw("Horizontal");

        //flips player direction when moving left/right
        if (directionX > 0.01f)
        {
            transform.localScale = Vector3.one;
        }

        else if (directionX < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();

        }

        anim.SetBool("run", directionX != 0);
        anim.SetBool("grounded", IsGrounded());

        //wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            
            rb.velocity = new Vector2(directionX * speed, rb.velocity.y);

            if (OnWall() && !IsGrounded())
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
            }
            else
            {
                rb.gravityScale = 3;
            }
            if (Input.GetButtonDown("Jump"))
            {
                Jump();

            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }

    }

    private void Jump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            AudioSource.Play();
            anim.SetTrigger("jump");
        }
        else if (OnWall() && !IsGrounded())
        {
            if (directionX == 0)
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            }
            wallJumpCooldown = 0;
        }
    }
        
    

    public bool canAttack()
    {
        return directionX == 0 && IsGrounded() && !OnWall();
    }
    private bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}
