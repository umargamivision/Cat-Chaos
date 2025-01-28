using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class VacuumPIgeon : MonoBehaviour
{
    public bool canVacuum;
    public float power;
    public Transform point1, point2;
    public Transform currentPoint;
    public float speed = 1;
    private void Start()
    {
        currentPoint = point1;
        transform.DOMove(currentPoint.position, speed).OnComplete(() =>
            reachPoint = true);
    }
    public bool reachPoint;
    public void ChangePosition()
    {
        if (reachPoint)
        {
            reachPoint = false;
            if (currentPoint == point1)
            {
                currentPoint = point2;
            }
            else
            {
                currentPoint = point1;
            }
            transform.DOMove(currentPoint.position, speed).OnComplete(() =>
            reachPoint = true);
        }
    }
    [InspectorButton]
    public void OnVacuumDetect()
    {
        power--;
        if (power <= 0)
        {
            canVacuum = true;
        }
        else
        {
            ChangePosition();
        }
    }
}
