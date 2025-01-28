using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using DG.Tweening.Core.Easing;
public class VaccumCleaner : MonoBehaviour
{
    public float vaccumSpeed;
    public float vaccumPower;
    public float vaccumRange;
    public float vaccumRadius;
    public float vaccumAngle;
    public Ease easeCurve;
    public Transform vaccumPoint;
    public LayerMask objectLayerMask;
    public List<Transform> vaccumObjects;
    public UnityEvent onvaccum;
    private void Update()
    {
        Startvaccum();
    }
    public void Startvaccum()
    {
        Collider[] colliders = Physics.OverlapSphere(vaccumPoint.position, vaccumRadius, objectLayerMask);
        foreach (var collider in colliders)
        {
            Vector3 directionToCollider = (collider.transform.position - vaccumPoint.position).normalized;
            float angle = Vector3.Angle(vaccumPoint.forward, directionToCollider);
            if (angle < vaccumAngle)
            {
                RaycastHit hit;
                if (Physics.Raycast(vaccumPoint.position, directionToCollider, out hit, vaccumRange, objectLayerMask))
                {
                    var vacuumPIgeon =collider.GetComponent<VacuumPIgeon>();
                    if(vacuumPIgeon != null)
                    {
                        vacuumPIgeon.OnVacuumDetect();
                    }
                    if (!vaccumObjects.Contains(collider.transform)&& vacuumPIgeon.canVacuum)
                    {
                        vaccumObjects.Add(collider.transform);
                        MoveTovaccumPoint(collider.transform);
                    }
                }
            }
        }
    }
    private Tweener moveTweener;

    private Coroutine followCoroutine;

    public void MoveTovaccumPoint(Transform vaccumObject)
    {
        // Stop any existing follow coroutine
        if (followCoroutine != null)
            StopCoroutine(followCoroutine);

        // Start a new follow coroutine
        followCoroutine = StartCoroutine(FollowvaccumPoint(vaccumObject));
    }

    private IEnumerator FollowvaccumPoint(Transform vaccumObject)
    {
        float elapsedTime = 0f;

        Vector3 startPosition = vaccumObject.position;

        vaccumObject.DOScale(Vector3.zero, vaccumSpeed/2).SetDelay(vaccumSpeed/2).SetEase(easeCurve);
        while (Vector3.Distance(vaccumObject.position, vaccumPoint.position) > 0.1f)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress using an easing function
            float t = elapsedTime / vaccumSpeed; // Normalize time (0 to 1)
            float easedT = EaseInQuint(t); // Apply easing

            // Move towards the updated vacuum point
            vaccumObject.position = Vector3.Lerp(startPosition, vaccumPoint.position, easedT);

            yield return null;
        }
        
        vaccumObjects.Remove(vaccumObject);
        onvaccum.Invoke();
    }

    // Custom easing function for InQuint
    private float EaseInQuint(float t)
    {
        return t * t * t * t * t;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(vaccumPoint.position, vaccumRadius);
        Gizmos.DrawRay(vaccumPoint.position, vaccumPoint.forward * vaccumRange);
        Vector3 rightDirection = Quaternion.AngleAxis(vaccumAngle, vaccumPoint.up) * vaccumPoint.forward;
        Vector3 leftDirection = Quaternion.AngleAxis(-vaccumAngle, vaccumPoint.up) * vaccumPoint.forward;
        Gizmos.DrawRay(vaccumPoint.position, rightDirection * vaccumRange);
        Gizmos.DrawRay(vaccumPoint.position, leftDirection * vaccumRange);
    }
}