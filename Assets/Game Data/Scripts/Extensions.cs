using UnityEngine;
using UnityEngine.AI;
//Nevmesh extenstions
public static class NavMeshAgentExtensions
{
    /// <summary>
    /// Checks if the NavMeshAgent has reached its destination.
    /// </summary>
    /// <param name="agent">The NavMeshAgent instance.</param>
    /// <param name="tolerance">An optional tolerance to account for floating-point errors.</param>
    /// <returns>True if the agent has reached its destination; otherwise, false.</returns>
    public static bool HasReachedDestination(this NavMeshAgent agent, float tolerance = 0.1f)
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude <= tolerance * tolerance)
            {
                return true;
            }
        }
        return false;
    }
}