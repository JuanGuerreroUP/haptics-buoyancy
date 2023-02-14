using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class AbstractFloatingObject<T>: MonoBehaviour, ILiquidReactive where T: Collider {
    
    public float density;

    private T col;
    private Rigidbody rb;
    private Liquid fluid;

    public abstract float GetObjVolume();
    public abstract float GetDisplacedVolume();

    public void Start() {
        this.rb = gameObject.GetComponent<Rigidbody>();
        this.col = this.GetComponent<T>();
        this.rb.mass = GetObjVolume() * this.density;
        //Debug.Log("Density of " + this.name + ": " +  this.GetDensity());
    }

    public void FixedUpdate() {
        this.rb.AddForce(GetNetForce());
    }

    public float GetH() {
        float? waterLevel = this.fluid?.GetWaterLevel();
        if(waterLevel == null){
            return 0;
        }
        float maxH = this.transform.localScale.y;
        float top = this.transform.position.y - (maxH / 2f);
        float h = waterLevel.Value - top;
        if( h <= 0) {
            h = 0;
        }
        Debug.Log(h);
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

    public T GetCollider() {
        return this.col;
    }

    [Obsolete]
    public float GetDensity()
    {
        return rb.mass / GetObjVolume();
    }

    private static Vector3 CalcForce(float volume, float density) { 
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
        Debug.Log(netForce);
        return netForce - GetDragForce();
    }
}
