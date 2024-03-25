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

    // Size boost variables
    [SerializeField] private float sizeBoostAmount = 2.5f; // Amount by which the player's size will increase
    [SerializeField] private float sizeBoostDuration = 5.0f; // Duration of the size boost

    private Vector3 originalScale; // Store the original scale of the player
    private Vector3 movementScale = Vector3.one; // Store the scale related to movement

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Store the original scale of the player
        originalScale = transform.localScale;
    }

    private void Update()
    {
        speed = 10;
        float directionX = Input.GetAxisRaw("Horizontal");

        // Update scale related to movement
        UpdateMovementScale(directionX);

        rb.velocity = new Vector2(directionX * (isSpeedBoosted ? speed + speedBoostAmount : speed), rb.velocity.y);

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

    private void UpdateMovementScale(float directionX)
    {
        if (directionX > 0.01f)
            movementScale = Vector3.one;
        else if (directionX < -0.01f)
            movementScale = new Vector3(-1, 1, 1);
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
        if (other.CompareTag("SizeBoost"))
        {
            StartCoroutine(ActivateSizeBoost());
            Destroy(other.gameObject); // Destroy the size boost power-up object
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

    private IEnumerator ActivateSizeBoost()
    {
        Debug.Log("Size boost activated");

        // Calculate the new scale based on the size boost amount
        Vector3 newSize = originalScale * sizeBoostAmount;

        // Increase the player's scale temporarily
        transform.localScale = newSize;

        // Wait for the duration of the size boost
        yield return new WaitForSeconds(sizeBoostDuration);

        // Reset the player's scale back to normal immediately
        transform.localScale = originalScale;
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
