using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TransformHelper))]
[RequireComponent(typeof(PhysicsHelper))]
public abstract class AbstractFloatingObject: MonoBehaviour, ILiquidReactive {
    
    public float density;
    private PhysicsHelper rb;
    private Liquid fluid;
    public TransformHelper transformHelper;

    public abstract float GetObjVolume();
    public abstract float GetDisplacedVolume();

    public void Awake() {
        this.rb = gameObject.GetComponent<PhysicsHelper>();
        this.transformHelper = GetComponent<TransformHelper>();
        this.rb.mass = GetObjVolume() * this.density;
        
        //Debug.Log("Density of " + this.name + ": " +  this.GetDensity());
    }

    public void FixedUpdate() {
        HIP hip = this.transform.parent == null ? null : this.transform.parent.gameObject.GetComponent<HIP>();
        if (hip == null){
            this.rb.GetRigidbody().AddForce(GetNetForce());
        }
    }

    public float GetH() {
        float? waterLevel = this.fluid?.GetWaterLevel();
        if(waterLevel == null){
            return 0;
        }
        float maxH = this.transformHelper.scale.y;
        float top = this.transformHelper.position.y - (maxH / 2f);
        float h = waterLevel.Value - top;
        if( h <= 0) {
            h = 0;
        }
        return h;
    }

    public void SetWater(Liquid water) {
        this.fluid = water;
    }
    public void UnsetWater(Liquid water) {
        if (this.fluid == water)
        {
            this.fluid = null;
        }
    }

    [Obsolete]
    public float GetDensity()
    {
        return rb.mass / GetObjVolume();
    }

    public static Vector3 CalcForce(float volume, float density) { 
        return volume * density * -Physics.gravity;
    }

    public Vector3 GetBuoyantForce()
    {
        return CalcForce(GetDisplacedVolume(), this.fluid?.density ?? 0);
    }

    public Vector3 GetWeightForce()
    {
        return CalcForce(GetObjVolume(), this.density);
    }

    public Vector3 GetDragForce()
    {
        float fluidDensity = this.fluid?.density ?? 0;//1.293f;
        return 0.5f * fluidDensity * this.rb.drag * this.rb.velocity * this.rb.velocity.magnitude;
    }

    public Vector3 GetNetForce() { 
        Vector3 netForce = this.GetBuoyantForce() - this.GetWeightForce();
        Debug.Log("NetF:" + netForce);
        return netForce - GetDragForce();
    }
}
