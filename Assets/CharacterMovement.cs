using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public AudioSource AudioSource;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello, world!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("space"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(0, 10, 0);
            AudioSource.Play();

            
        }

        if (Input.GetKey("d") || Input.GetKey("w"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(2, 0, 0);
        }

        if (Input.GetKey("a") || Input.GetKey("s"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(-2, 0, 0);
        }

    }
}
