
using UnityEngine;

public abstract class GrannyBaseState
{
    public abstract void EnterState(GrannyStateManager stateManager);
    public abstract void UpdateState(GrannyStateManager stateManager);
    public abstract void ExitState(GrannyStateManager stateManager);
    public abstract void OnCollisionEnter(GrannyStateManager stateManager, Collision collision);
}
