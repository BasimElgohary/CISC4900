using UnityEngine;

public class Enemy : Entity
{
    private bool playerDetected;

    protected override void Update() {
        HandleCollision();
        HandleMovement();
        HandleAnimation();
        HandleFlip();
        HandleAttack();
    }
    protected override void HandleMovement() {
        if (canMove == true)
            rb.linearVelocity = new Vector2(facingDirection * moveSpeed, rb.linearVelocity.y);
        else 
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        //change velocity in the x direction based on input and keep the y velocity the same
    }
    protected override void HandleAttack() {
        if (playerDetected) {
            animate.SetTrigger("attack");
        }
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsTarget);
        //checks attack point position with radius and layer to see if object is player and whether or not it is in range
    }

    protected override void Die()
    {
        animate.enabled = false;
        col.enabled = false;

        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);
        UI.instance.UpdateKillCount();
    }
}


    

