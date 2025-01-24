using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class VacumeCleaner : MonoBehaviour
{
    public LayerMask objectLayerMask;
    public List<Transform> vacumeObjects;
    public Transform vacumePoint;
    [InspectorButton]
    public void MoveToVacumePoint(Transform vacumeObject)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(vacumeObject.DOMove(vacumePoint.position, 2f));
        sequence.Join(vacumeObject.DOScale(Vector3.zero, 1f));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (vacumeObjects.Contains(other.transform))
        {
            MoveToVacumePoint(other.transform);
        }
    }
}