using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        if (agent.isOnNavMesh) 
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            agent.acceleration = 8;
        }
    }
}