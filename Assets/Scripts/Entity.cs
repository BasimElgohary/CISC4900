using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour      
{
    protected float xInput;
    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] private float jumpForce = 8f;
    protected int facingDirection = 1; //1 is right, -1 is left
    protected Rigidbody2D rb;
    protected Animator animate;
    protected SpriteRenderer sr; 
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int currentHealth = 1;
    
    [Header("Attack Details")]
    [SerializeField] protected float attackRadius; 
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask whatIsTarget;
    [SerializeField] protected bool facingRight = true;
    
    [Header("Movement details")] //Provides a header in the Unity Inspector for better organization 
    [Header("Collision Details")] 
    [SerializeField] private float groundCheckDistance;
    [SerializeField] protected bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float damageFeedbackDuration = 0.2f;
    private Coroutine damageFeedbackCoroutine;
    private bool canJump = true;
    protected bool canMove = true;
    protected Collider2D col;
    
    protected virtual void Awake() //Called when the script instance is being loaded before the Start() function; used to set up references like rigidbody and animator
    {
        rb = GetComponent<Rigidbody2D>();
        animate = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
        col = GetComponent<Collider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        
    }
    protected virtual void Update() {  //Called once per frame
        HandleCollision();
        HandleMovement();
        HandleInput();
        HandleAnimation();
        HandleFlip();
    }

    public void EnableMovementAndJump(bool enable) {
        canJump = enable;
        canMove = enable;
    }

     private void JumpAttempt() {
        if (isGrounded && canJump)
         rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
          //change velocity in the y direction based on the jumpForce value and keep the x velocity the same
    }

     private void HandleInput() {
        //get horizontal input
        xInput = Input.GetAxisRaw("Horizontal"); 
       
        //handles jump/vertical input
        if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyUp(KeyCode.UpArrow))) {
            JumpAttempt();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            HandleAttack();
        }
    }
     protected void HandleAnimation() {
        // bool isMoving = rb.linearVelocity.x != 0;
        animate.SetFloat("xVelocity", rb.linearVelocity.x);
        animate.SetBool("isGrounded", isGrounded);
        animate.SetFloat("yVelocity", rb.linearVelocity.y);
    }

     protected virtual void HandleAttack() {
        if (isGrounded) {
            animate.SetTrigger("attack");
            // rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    public void DamageTargets()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsTarget); 
        //checks where you are attacking from, the radius of the attack, and what is considered an enemy
        //returns an array of colliders that are within the attack radius and belong to the enemy layer
        foreach (Collider2D enemy in enemyColliders)
        {
            Entity entityTarget = enemy.GetComponent<Entity>();
            entityTarget.TakeDamage();
        }
    }

    private void TakeDamage() {

        PlayDamageFeedback();

        currentHealth = currentHealth - 1;
        if (currentHealth <= 0) {
            Die();
        }
        Debug.Log(gameObject.name + " Entity took damage");

    }

    private void PlayDamageFeedback() {                  //prevents overlapping damage feedback coroutines
        if(damageFeedbackCoroutine != null) {
            StopCoroutine(damageFeedbackCoroutine); 
        }
        StartCoroutine(DamageFeedbackCoroutine());
    }
    private IEnumerator DamageFeedbackCoroutine() {
        Material originalMaterial = sr.material;
        sr.material = damageMaterial;
        yield return new WaitForSeconds(damageFeedbackDuration);
        sr.material = originalMaterial;
    }   

    protected virtual void Die()
    {
        animate.enabled = false;
        col.enabled = false;

        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);
        UI.instance.EnableGameOverUI();
    }

    protected virtual void HandleMovement() {
        if (canMove == true)
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
        else 
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        //change velocity in the x direction based on input and keep the y velocity the same
    }

   
    [ContextMenu("Flip")] //provides a way to test the Flip function in the Unity Editor
    public void Flip()                  //The character will be facing right by default
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight; 
        facingDirection *= -1;
    }

    protected virtual void HandleFlip() {   
        if (xInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (xInput < 0 && facingRight)
        {
            Flip();
        }
    }
     //Handles the flipping of the character based on movement direction; 
    // If movement is increasing in the positive x direction and the character is not facing right, flip the character.
    // If movement is decreasing in the x direction and the character is facing right, flip the character.

    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer); 
        //casts a ray downward (Vector2.down) from the player's position (transform.position) to check for ground collision with distnance defined by 
        //groundCheckDistance with consideration only given to objects in the groundLayer
    }

    private void OnDrawGizmos() //Visualizes the ground check distance in the Unity Editor when the object is selected
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance)); 
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        // if (attackPoint != null)
        // {
        //     Gizmos.DrawWireSphere(attackPoint.position, attackRadius); //For object to protect to prevent null reference error
        // }
    //transform.position is the current position of the player object (where the line starts)
    //wire sphere is the visual representation of the attack radius around the attack point
    }
}

