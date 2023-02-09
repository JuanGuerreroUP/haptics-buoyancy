using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    private readonly List<GameObject> inCollision = new List<GameObject>();
    private BoxCollider waterCollider;
    public float waterLevel;

    private void Start(){
        this.waterCollider = GetComponent<BoxCollider>();
        this.waterCollider.center = new Vector3(0, waterLevel / 2, 0);
        this.waterCollider.size = new Vector3(10, waterLevel, 10);
        this.transform.GetChild(0).transform.localPosition = new Vector3(0, waterLevel, 0);
    }

    void FixedUpdate()
    {
        foreach(GameObject gameObject in inCollision){
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddForce(Physics.gravity*-2f, ForceMode.Force);
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        Debug.Log("Collided!");
        inCollision.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        inCollision.Remove(other.gameObject);
    }
}
