using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Events;

public class IntractableDetector : MonoBehaviour
{
    public Grabbable currentGrabbable;
    public LayerMask layerMask;
    [SerializeField] private Transform origin;

    [Header("Settings")]
    [SerializeField] private float detectionLength = 5f;
    private Ray ray;
    public IDetectable currentDetectedObject;
    public UnityEvent<RaycastHit> onRayCast;
    public UnityEvent<IGrabbable> onGrabableHit;
    public UnityEvent<ISwitch> onDoorDetected;
    public UnityEvent<ClothTear> onClothTearDetected;
    public UnityEvent<CatBed> onCatBedDetected;
    public UnityEvent<SpecialItem> onSpecialItemDetected;
    private void Update()
    {
        PerformDetection();
    }
    private void PerformDetection()
    {
        RaycastHit raycastHit;
        ray.origin = origin.position;
        ray.direction = origin.forward;

        ISwitch iSwitch = null;
        IGrabbable iGrabbable = null;
        ClothTear clothTear = null;
        CatBed catBed = null;
        SpecialItem specialItem = null;

        if (Physics.Raycast(ray, out raycastHit, detectionLength, layerMask))
        {
            //Debug.Log("ray hit with "+raycastHit.collider.name);
            currentDetectedObject = raycastHit.collider.GetComponent<IDetectable>();

            iGrabbable = raycastHit.collider.GetComponent<IGrabbable>();
            if (iGrabbable != null)
            {
                UpdateCurrentGrabbable(iGrabbable);
            }
            onGrabableHit.Invoke(iGrabbable);

            iSwitch = raycastHit.collider.GetComponent<ISwitch>();
            onDoorDetected.Invoke(iSwitch);
        
            clothTear = raycastHit.collider.GetComponent<ClothTear>();
            onClothTearDetected.Invoke(clothTear);
            
            catBed = raycastHit.collider.GetComponent<CatBed>();
            onCatBedDetected.Invoke(catBed);

            specialItem = raycastHit.collider.GetComponent<SpecialItem>();
            onSpecialItemDetected.Invoke(specialItem);
        }
        else
        {
            currentDetectedObject = null;

            onGrabableHit.Invoke(iGrabbable);
            onDoorDetected.Invoke(iSwitch);
            onClothTearDetected.Invoke(clothTear);
            onCatBedDetected.Invoke(catBed);
            onSpecialItemDetected.Invoke(specialItem);
        }
    }
    public void UpdateCurrentGrabbable(IGrabbable grabbable)
    {
        if(currentGrabbable==grabbable)return;
        if(currentGrabbable)currentGrabbable.OnFocus(false);
        currentGrabbable = grabbable as Grabbable;
    }
    private void OnDrawGizmosSelected()
    {
        if (origin == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin.position, origin.position + origin.forward * detectionLength);
    }
}
public interface IDetectable
{
    Transform transform { get; }
    void OnDetected();
}
