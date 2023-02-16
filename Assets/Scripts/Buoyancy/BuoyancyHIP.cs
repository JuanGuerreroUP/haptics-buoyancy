using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HIP))]
public class BuoyancyHIP : MonoBehaviour
{
    public Transform prevParent, grabbedObj;
    private Material material;
    private HIP hip;
    private bool isGrabbed;
    private void Awake()
    {
        isGrabbed = false;
        hip = this.GetComponent<HIP>();
        material = hip.GetInteractionPoint().GetComponent<Renderer>().material;
    }
    private void Update()
    {
        if (hip.IsButtonPressed(0))
        {
            material.color = Color.red;
        }
        else
        {
            material.color = Color.white;
            if (grabbedObj != null && isGrabbed)
            {
                isGrabbed = false;
                grabbedObj.parent = prevParent;
                prevParent = null;
                grabbedObj.GetComponent<Rigidbody>().isKinematic = false;
                grabbedObj = null;
            }
        }
    }
    private void FixedUpdate()
    {

    }
    private void OnCollisionStay(Collision other)
    {
        if (hip.IsButtonPressed(0) && !isGrabbed)
        {
            if (other.rigidbody.constraints != UnityEngine.RigidbodyConstraints.FreezeAll)
            {
                isGrabbed = true;
                prevParent = other.transform.parent;
                grabbedObj = other.transform;
                other.transform.parent = transform;
                other.rigidbody.isKinematic = true;
            }
        }
    }
    private void OnCollisionExit(Collision other)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
