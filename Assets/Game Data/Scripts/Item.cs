using System.Collections;
using System.Collections.Generic;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [Tooltip("Set Layer Intractable or item")]
    public ItemType itemType;
    public Grabbable grabbable;
    public Breakable breakable;
    public HUDNavigationElement navigationElement;
    [InspectorButton]
    public void Set_Item(bool isBreakable)
    {
        gameObject.layer = LayerMask.NameToLayer("Item");
        navigationElement = GetComponentInChildren<HUDNavigationElement>();
        if (navigationElement) navigationElement.enabled = false;
        if (navigationElement == null)
        {
            //navigationElement = gameObject.AddComponent<HUDNavigationElement>();
        }
        grabbable = GetComponent<Grabbable>();
        if (grabbable == null)
        {
            grabbable = gameObject.AddComponent<Grabbable>();
        }
        breakable = GetComponent<Breakable>();
        if (breakable == null && isBreakable)
        {
            breakable = gameObject.AddComponent<Breakable>();
        }
        var rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        grabbable.rb = rb;
        if (grabbable.outline == null)
        {
            var outL = GetComponent<QuickOutline>();
            if (outL == null)
            {
                outL = gameObject.AddComponent<QuickOutline>();
                outL.OutlineColor = Color.green;
                outL.OutlineWidth = 8;
                outL.OutlineMode = QuickOutline.Mode.OutlineVisible;
            }
            grabbable.outline = outL;
        }
        // Set MeshCollider 
        var anyCollider = GetComponent<Collider>();
        if (anyCollider == null)
        {

            var collider = GetComponent<MeshCollider>();
            if (collider == null)
            {
                collider = gameObject.AddComponent<MeshCollider>();
                collider.convex = true;
            }
            if (collider.sharedMesh == null)
            {
                collider.sharedMesh = GetComponentInChildren<MeshFilter>().sharedMesh;
                //collider.sharedMesh=GetComponentInChildren<MeshFilter>().mesh;
            }
        }
    }
    private void OnEnable() 
    {
        grabbable.OnGrab.AddListener(()=>ShowIndicator(false));
    }
    private void OnDisable() 
    {
        grabbable.OnGrab.RemoveListener(()=>ShowIndicator(false));    
    }
    public void ShowIndicator(bool show)
    {
        if (navigationElement != null)
        {
            navigationElement.gameObject.SetActive(show);
        }
    }
}
