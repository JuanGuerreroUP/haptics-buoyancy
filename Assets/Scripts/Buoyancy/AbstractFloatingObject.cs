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
    private ForceViewer forceViewer;

    public abstract float GetObjVolume();
    public abstract float GetDisplacedVolume();

    public void Awake()
    {
        this.rb = GetComponent<PhysicsHelper>();
        this.transformHelper = GetComponent<TransformHelper>();
        this.forceViewer = FindObjectOfType<ForceViewer>();
    }
    public void Start()
    {
        this.rb.Mass = GetObjVolume() * this.density;
    }

    public void FixedUpdate()
    {
        HIP hip = this.transform.parent == null ? null : this.transform.parent.gameObject.GetComponent<HIP>();
        if (hip == null)
        {
            Vector3 netForce = GetNetForce(this.forceViewer);
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

    public void SetWater(Liquid water)
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

    [Obsolete]
    public float GetDensity()
    {
        return rb.Mass / GetObjVolume();
    }

    public static Vector3 CalcForce(float volume, float density)
    {
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
        return 0.5f * fluidDensity * this.rb.Drag * this.rb.Velocity * this.rb.Velocity.magnitude;
    }

    public Vector3 GetNetForce(ForceViewer forceViewer)
    {
        Vector3 buoyantForce = this.GetBuoyantForce();
        Vector3 weight = this.GetWeightForce();
        Vector3 dragForce = GetDragForce();
        Vector3 netForce = buoyantForce - weight;
        Debug.Log("NetF:" + netForce);
        if (forceViewer != null)
        {
            forceViewer.SetForces(buoyantForce, weight, dragForce, netForce);
        }
        return netForce - dragForce;
    }
    public Vector3 GetNetForce()
    {
        return this.GetNetForce(null);
    }
}