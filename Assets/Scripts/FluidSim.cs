using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FluidSim : MonoBehaviour
{
    private List<Rigidbody> physicObjects = new List<Rigidbody>();
    private HIP hip;
   

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if(rb != null)
        {
            Debug.Log(rb.gameObject.name);
            this.physicObjects.Add(rb);
        }
        HIP hip = other.gameObject.GetComponent<HIP>();
        if(hip != null)
        {
            this.hip = hip;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            this.physicObjects.Remove(rb);
        }
        HIP myhip = other.gameObject.GetComponent<HIP>();
        if (myhip != null)
        {
            this.hip = null;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public HIP getHIP()
    {
        return this.hip;
    }

    private void FixedUpdate()
    {
        foreach(Rigidbody rb in physicObjects)
        {
            rb.AddForce(Vector3.up * 1000, ForceMode.Force);
        }
    }
}
