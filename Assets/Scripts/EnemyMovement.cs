using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Pesos de Terrenos (Mayor número = más evita pasar por ahí)")]
    public float fireCost = 1f;
    public float coldCost = 1f;
    public float lightningCost = 1f;
    public float poisonCost = 1f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.SetAreaCost(NavMesh.GetAreaFromName("Fire_Tile"), fireCost);
        agent.SetAreaCost(NavMesh.GetAreaFromName("Cold_Tile"), coldCost);
        agent.SetAreaCost(NavMesh.GetAreaFromName("Lightning_Tile"), lightningCost);
        agent.SetAreaCost(NavMesh.GetAreaFromName("Poison_Tile"), poisonCost);

        GameObject targetTile = GameObject.FindGameObjectWithTag("Finish");

        if (targetTile != null)
        {
            agent.SetDestination(targetTile.transform.position);
        }
    }
}
