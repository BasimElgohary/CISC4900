using UnityEngine;

public class Player : MonoBehaviour
{
    private float xInput;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float jumpForce = 8f;
    private Rigidbody2D rb;
    private Animator animate;
    [SerializeField] private bool facingRight = true;
    
    [Header("Movement details")] //Provides a header in the Unity Inspector for better organization 
    [Header("Collision Details")] 
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    
    private void Awake() //Called when the script instance is being loaded before the Start() function; used to set up references like rigidbody and animator
    {
        rb = GetComponent<Rigidbody2D>();
        animate = GetComponentInChildren<Animator>();
    }
    private void Update() {  //Called once per frame
        HandleCollision();
        HandleMovement();
        HandleInput();
        HandleAnimation();
        HandleFlip();
    }

     private void Jump() {
        if (isGrounded)
         rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
          //change velocity in the y direction based on the jumpForce value and keep the x velocity the same
    }

     private void HandleAnimation() {
        bool isMoving = rb.linearVelocity.x != 0;
        animate.SetBool("isMoving", isMoving);
        animate.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private void HandleMovement() {
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
        //change velocity in the x direction based on input and keep the y velocity the same
    }

    private void HandleInput() {
        //get horizontal input
        xInput = Input.GetAxisRaw("Horizontal"); 
       
        //handles jump/vertical input
        if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyUp(KeyCode.UpArrow))) {
            Jump();
        }
    }
    [ContextMenu("Flip")] //provides a way to test the Flip function in the Unity Editor
    private void Flip()                  //The character will be facing right by default
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight; 
    }

    private void HandleFlip() {   
        if (xInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (xInput < 0 && facingRight)
        {
            Flip();
        }
    }

    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer); 
        //casts a ray downward (Vector2.down) from the player's position (transform.position) to check for ground collision with distnance defined by 
        //groundCheckDistance with consideration only given to objects in the groundLayer
    }

    //Handles the flipping of the character based on movement direction; 
    // If movement is increasing in the positive x direction and the character is not facing right, flip the character.
    // If movement is decreasing in the x direction and the character is facing right, flip the character.

    private void OnDrawGizmos() //Visualizes the ground check distance in the Unity Editor when the object is selected
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance)); 
    }
    //transform.position is the current position of the player object (where the line starts)
}

