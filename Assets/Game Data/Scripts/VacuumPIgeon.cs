using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class VacuumPIgeon : MonoBehaviour
{
    public bool canVacuum;
    public float power;
    public Transform point1, point2;
    public Transform currentPoint;
    public float speed = 1;
    public Image healthBar;
    private void Start()
    {
        currentPoint = point1;
        transform.DOMove(currentPoint.position, speed).OnComplete(() =>
            reachPoint = true);
    }
    public void UpdateHealth()
    {
        healthBar.fillAmount = power / 300;
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
            UpdateHealth();
            ChangePosition();
        }
    }
}
