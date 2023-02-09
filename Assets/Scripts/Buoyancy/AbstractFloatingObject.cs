using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFloatingObject: MonoBehaviour {
    public abstract float GetObjVolume();
    public float density;
    public Rigidbody rb;

    public float GetH(float waterLevel){
        float maxH = this.transform.localScale.y;
        float top = this.transform.position.y + (maxH / 2f);
        float h = waterLevel - top;
        if( h <= 0) {
            return 0;
        }
        return h;
    }

    public float getObjDensity()
    {
        return rb.mass / GetObjVolume();
    }
    private void Start()
    {
            this.rb = gameObject.GetComponent<Rigidbody>();
    }


    public Vector3 GetForce(float waterLevel)
    {
        float displacedFluid;
        displacedFluid = GetObjVolume() * GetH(waterLevel) * 1;
        Vector3 force = displacedFluid * -Physics.gravity;
        Debug.Log(force);
        return force;
    }
    
    public void ApplyForce(float waterLevel)
    {
        //this.rb.useGravity = false;
        this.rb.AddForce(GetForce(waterLevel));
    }
}
