using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    private readonly List<AbstractFloatingObject> inCollision = new List<AbstractFloatingObject>();
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
        foreach(AbstractFloatingObject gameObject in inCollision){
            gameObject.ApplyForce(waterLevel);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AbstractFloatingObject absF = other.gameObject.GetComponent<AbstractFloatingObject>();
        if (absF != null)
        {
            Debug.Log(absF.gameObject.name);
            this.inCollision.Add(absF);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        AbstractFloatingObject absF = other.gameObject.GetComponent<AbstractFloatingObject>();
        if (absF != null)
        {
            Debug.Log(absF.gameObject.name);
            inCollision.Remove(absF);
        }
    }
}
