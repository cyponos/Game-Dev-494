using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    private Animator anim;
    private bool grounded;
    private int jumpsRemaining = 2;
    public AudioSource AudioSource;

    // Speed boost variables
    [SerializeField] private float speedBoostAmount = 5.0f;
    [SerializeField] private float speedBoostDuration = 5.0f;
    private bool isSpeedBoosted = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        speed = 10;
        float directionX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(directionX * (isSpeedBoosted ? speed + speedBoostAmount : speed), rb.velocity.y);

        if (directionX > 0.01f)
            transform.localScale = Vector3.one;
        else if (directionX < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded && jumpsRemaining > 0)
            {
                Jump();
                jumpsRemaining--;
            }
            else if (!grounded && jumpsRemaining == 1)
            {
                Jump();
                jumpsRemaining--;
            }
        }

        anim.SetBool("run", directionX != 0);
        anim.SetBool("grounded", grounded);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 10);
        AudioSource.Play();
        anim.SetTrigger("jump");
        grounded = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpeedBoost"))
        {
            StartCoroutine(ActivateSpeedBoost());
            Destroy(other.gameObject); // Destroy the speed boost power-up object
        }
    }

    private IEnumerator ActivateSpeedBoost()
    {
        if (!isSpeedBoosted)
        {
            isSpeedBoosted = true;
            yield return new WaitForSeconds(speedBoostDuration);
            isSpeedBoosted = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            jumpsRemaining = 2;
        }
    }
}
