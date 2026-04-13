using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform goal;
    public float detectionRange = 10f;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, goal.position);

        if (distance < detectionRange)
        {
            agent.SetDestination(goal.position);
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
