using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TransformHelper))]
[RequireComponent(typeof(PhysicsHelper))]
public abstract class AbstractFloatingObject : MonoBehaviour, ILiquidReactive
{

    public float density;
    private PhysicsHelper rb;
    private Liquid fluid;
    protected TransformHelper transformHelper;

    private readonly BuoyantState state = new BuoyantState();
    public BuoyantState State { get => this.state; }

    public abstract float GetObjVolume();
    public abstract float GetDisplacedVolume();
    protected abstract float GetDragArea();

    public void Awake()
    {
        this.rb = GetComponent<PhysicsHelper>();
        this.transformHelper = GetComponent<TransformHelper>();
        this.OnAwake();
    }
    public void Start()
    {
        this.rb.Mass = GetObjVolume() * this.density;
    }

    protected virtual void OnAwake() { }

    private static Vector3 RoundVector(Vector3 value, float precision){
        value *= precision;
        for(int i = 0; i < 3; i++){
            value[i] = Mathf.Round(value[i]);
            value[i] /= precision;
        }
        return value;
    }

    public void FixedUpdate()
    {
        HIP_SM hip = this.transform.parent == null ? null : this.transform.parent.gameObject.GetComponent<HIP_SM>();
        if (hip == null)
        {
            Vector3 netForce = GetNetForce();//RoundVector(, 1.0f);
            this.rb.GetRigidbody().AddForce(netForce);
        }
    }

    public float GetH()
    {
        if (this.fluid == null)
        {
            return 0;
        }
        float maxH = this.transformHelper.Scale.y;
        float top = this.transformHelper.Position.y - (maxH / 2f);
        float? h = this.fluid?.GetDepth(top);
        Debug.Log("Depth" + h);
        if (!h.HasValue || h.Value <= 0)
        {
            h = 0;
        }
        return h.Value;
    }

    public virtual void SetWater(Liquid water)
    {
        this.fluid = water;
    }
    public void UnsetWater(Liquid water)
    {
        if (this.fluid == water)
        {
            this.fluid = null;
        }
    }

    private static Vector3 CalcForce(float volume, float density)
    {
        return volume * density * -Physics.gravity;
    }

    private Vector3 GetBuoyantForce()
    {
        this.state.DisplacedVolume = GetDisplacedVolume();
        return CalcForce(this.state.DisplacedVolume, this.fluid?.density ?? 0);
    }

    private Vector3 GetWeightForce()
    {
        return CalcForce(GetObjVolume(), this.density);
    }

    private Vector3 GetDragForce()
    {
        float fluidDensity = 0f;
        if (this.fluid != null){
            fluidDensity = this.fluid.density;
        }
        Vector3 pV = fluidDensity * this.rb.Velocity.magnitude * this.rb.Velocity;
        float area = this.GetDragArea();
        Vector3 dragForce = this.rb.Drag * area * (pV/2.0f);
        return dragForce;
    }

    public Vector3 GetNetForce()
    {
        this.state.BouyantForce = this.GetBuoyantForce();
        this.state.Weight = this.GetWeightForce();
        this.state.DragForce = this.GetDragForce();
        this.state.NetForce = this.state.BouyantForce - this.state.Weight;
        if(this.state.DragForce.y < 0){
            return this.state.NetForce;
        }
        return this.state.NetForce - this.state.DragForce;
    }
}