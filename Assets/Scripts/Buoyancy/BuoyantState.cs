using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyantState
{
    public MinMax<float> displacedVolumeLimits;
    public MinMax<float> netForceLimits;

    private Vector3 netForce;
    private float displacedVolume;
   
    public Vector3 BouyantForce { get; set; }
    public Vector3 Weight { get; set; }
    public Vector3 DragForce { get; set; }
    public Vector3 NetForce { 
        get => this.netForce; 
        set {
            this.netForceLimits.Set(value.y);
            this.netForce = value;
        } 
    }

    public float DisplacedVolume {
        get => this.displacedVolume;
        set {
            this.displacedVolumeLimits.Set(value);
            this.displacedVolume = value;
        }
    }
}