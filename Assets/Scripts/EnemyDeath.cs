using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public void PlayDeath()
    {
        this.GetComponent<EnemyMovement>().enabled = false;
        this.GetComponent<Enemy_Attack>().enabled = false;
        this.GetComponent<CapsuleCollider>().enabled = false;
        
        //Play animation once here, no loop. Might set up timer for destruction later, idk
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Die");
        gameObject.GetComponent<Billboard_Behavior>().enabled = false;
        
        float deathLength = anim.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, deathLength);
    }
}
