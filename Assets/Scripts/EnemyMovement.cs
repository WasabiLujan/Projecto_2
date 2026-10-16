using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();


        GameObject targetTile = GameObject.FindGameObjectWithTag("Finish");


        if (targetTile != null)
        {
            agent.SetDestination(targetTile.transform.position);
        }
        else
        {
            Debug.LogError("No eta");
        }
    }
}
