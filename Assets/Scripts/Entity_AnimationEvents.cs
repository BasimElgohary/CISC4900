using UnityEngine;

public class Entity_AnimationEvents : MonoBehaviour
{
    private Entity entity;
    private void Awake(){
        entity = GetComponentInParent<Entity>();
    }

    public void DamageTargets() {
         entity.DamageTargets();
     }

    private void EnableMovementAndJump() {
        entity.EnableMovementAndJump(true);
    }
 
    private void DisableMovementAndJump() {
        entity.EnableMovementAndJump(false);
    }
}
