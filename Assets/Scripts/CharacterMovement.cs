using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    private Animator anim;
    private Boolean grounded;
    
    public AudioSource AudioSource;
    // Start is called before the first frame update
    private void Start()
    {
        //grab references for rigidbody and animator
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        speed = 10;
        float directionX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(directionX * speed, rb.velocity.y);

        if (directionX > 0.01f)
        {
            transform.localScale = Vector3.one;
        }

        else if (directionX < -0.01f)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
     
        if (Input.GetButtonDown("Jump") && grounded)
        {
            Jump();
            
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }

    }
}
