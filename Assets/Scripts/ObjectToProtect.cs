using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectToPrtect : Entity
{
    [Header("Object To Protect Details")]
    [SerializeField] private Transform player;
    protected override void Update()
    {
        HandleFlip();
    }
    //tracks player transform position to flip girl character to always face player 
    protected override void HandleFlip()
    {
         if (player.transform.position.x > transform.position.x && !facingRight) 
        {
            Flip();
        }
        else if (player.transform.position.x < transform.position.x && facingRight)
        {
            Flip();
        }
    }

    protected override void Die()
    {
        base.Die();
        UI.instance.EnableGameOverUI();
    }
    
}
