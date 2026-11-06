using UnityEngine;
using UnityEngine.AI;

public class MoveAgentToPosition : MonoBehaviour
{
    [SerializeField] private Vector3 endPosition;

    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(endPosition);
    }
}
