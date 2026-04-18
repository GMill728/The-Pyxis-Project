using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public GameObject[] goals;
    public float detectionRange = 10f;
    public float stoppingDistance = 30f;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        goals = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, goals[0].transform.position);

        if (distance < detectionRange && distance > stoppingDistance)
        {
            agent.SetDestination(goals[0].transform.position);
        }
        else
        {
            // Optionally stop the agent
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                // Do nothing, let it finish moving or stop instantly
            }
            agent.ResetPath(); // Stops agent
        }
    }
}
