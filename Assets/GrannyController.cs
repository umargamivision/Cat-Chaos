using System.Collections;
using System.Collections.Generic;
using Ommy.Attribute;
using UnityEngine;
public class GrannyController : MonoBehaviour
{
    public GrannyStateManager grannyStateManager;
    [InspectorButton]
    public void ChasePlayer()
    {
        grannyStateManager.SwitchState(grannyStateManager.grannyChasingState);
    }
}