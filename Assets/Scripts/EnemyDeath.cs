using UnityEngine;
using UnityEngine.AI;

public class EnemyDeath : MonoBehaviour
{
    [SerializeField]
    private GameObject pickupPrefab;

    private TimeManager _timeManager;

    private void Awake()
    {
        _timeManager = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<TimeManager>();
    }

    public void PlayDeath()
    {
        this.GetComponent<EnemyMovement>().enabled = false;
        this.GetComponent<Enemy_Attack>().enabled = false;
        this.GetComponent<CapsuleCollider>().enabled = false;
        this.GetComponent<NavMeshAgent>().enabled = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        if (Mathf.FloorToInt(Random.Range(0, 300)) == 0)
        {
            Instantiate(pickupPrefab, this.transform.position, new Quaternion()).GetComponent<Pickup_Handler>().pickupType = Pickup_Handler.PickupType.Key;
        }

        _timeManager.timeLeft += 3f;

        //Play animation once here, no loop. Might set up timer for destruction later, idk
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Die");
        gameObject.GetComponent<Billboard_Behavior>().enabled = false;
        
        float deathLength = anim.GetCurrentAnimatorStateInfo(0).length + 1;
        Destroy(gameObject, deathLength);
    }
}
