using UnityEngine;
using UnityEngine.AI;

public class MoveToTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    private NavMeshAgent agent;

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 3;
    }

    void Update()
    {
        agent.SetDestination(target.position);
    }
}
