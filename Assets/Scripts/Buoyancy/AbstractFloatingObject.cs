using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFloatingObject: MonoBehaviour {
    public abstract float CalcVolume(float waterLevel);
    public float density;
    public Rigidbody rb;

    public float GetH(float waterLevel){
        float maxH = this.transform.localScale.y;
        float bottom = this.transform.position.y - (maxH / 2f);
        float h = waterLevel - bottom;
        if (h >= maxH) {
            return maxH;
        } else if( h <= 0) {
            return 0;
        }
        return h;
    }
    private void Start()
    {
            this.rb = gameObject.GetComponent<Rigidbody>();
    }

    public Vector3 GetForce(float waterLevel)
    {
        Vector3 force = this.density * this.CalcVolume(waterLevel) * -Physics.gravity;
        Debug.Log(force);
        return force;
    }
    
    public void ApplyForce(float waterLevel)
    {
        this.rb.useGravity = false;
        this.rb.AddForce(GetForce(waterLevel));
    }
}
