using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Worq;

public class WayPointSystem : MonoBehaviour
{
    public enum LoopType
    {
        Reverse, Loop, Once
    }
    public LoopType loopType;
    public int currentWayPoint;
    public List<Transform> wayPoints;
    public Vector3 GetWayPoint()
    {
        return wayPoints[currentWayPoint].position;
    }
    public void OnReachWayPoint()
    {
        UpdateWayPoint();
    }
    void UpdateWayPoint()
    {
        currentWayPoint++;
        if (currentWayPoint >= wayPoints.Count)
        {
            switch (loopType)
            {
                case LoopType.Loop:
                    currentWayPoint = 0;
                    break;
                case LoopType.Reverse:
                    wayPoints.Reverse();
                    currentWayPoint = 0;
                    break;
                case LoopType.Once:
                    currentWayPoint = wayPoints.Count-1;
                    break;
            }
        }
    }
}
