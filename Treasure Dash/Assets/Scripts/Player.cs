using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource collected, hit;
    [SerializeField] private Vector3 checkpoint;

    private Rigidbody2D body;
    private SpriteRenderer sr;
    public ItemCounter ic;
    // Start is called before the first frame update
    void Start() {
        body = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        checkpoint = transform.position;
    }

    // Update is called once per frame
    void Update() {
        keyboardPress();
    }

    // This method receives keyboard input and moves the player character
    void keyboardPress() {
        // Initialize variables for use in movement
        float speed = 2.5f;
        float yMove = 0;
        float xMove = 0;
        // Retrieve the keyboard input from the player
        xMove = Input.GetAxisRaw("Horizontal");
        yMove = Input.GetAxisRaw("Vertical");
        // Move the player character accordingly
        transform.position += new Vector3(xMove, yMove, 0) * Time.deltaTime * speed;
        // Animate the player character to match the input given
        animateCharacter(xMove, yMove);
    }

    // This method animates the player character according to given parameters
    void animateCharacter(float x, float y) {
        
        // Determine which side of the screen the character is facing
        // Set te animator's parameter to activate the run animation
        if (x > 0) {
            sr.flipX = false;
            animator.SetFloat("Speed", x);
        } else if(x < 0) {
            sr.flipX = true;
            animator.SetFloat("Speed", Mathf.Abs(x));
        } else {
            // If the character is running up or down, the y value is used instead
            animator.SetFloat("Speed", Mathf.Abs(y));
        }
    }
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Diamond")) {
            Destroy(other.gameObject);
            collected.Play();
            ic.gemCount++;
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Enemy"))
        {
            transform.position = checkpoint;
            hit.Play();
        }
    }
}