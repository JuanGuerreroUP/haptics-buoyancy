using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsHelper : MonoBehaviour
{
    private Rigidbody rb;
    private float mass;


    public float Drag
    {
        get; private set;
    }
    public Vector3 Velocity
    {
        get; private set;
    }
    public float Mass
    {
        get { return this.mass; }
        set { 
            this.rb.mass = value;
            this.UpdateValues();
        }
    }

    private void UpdateValues() {
        this.Drag = this.rb.drag;
        this.Velocity = this.rb.velocity;
        this.mass = this.rb.mass;
    }

    void Awake()
    {
        this.rb = GetComponent<Rigidbody>();
        this.UpdateValues();
    }

    void FixedUpdate()
    {
        this.UpdateValues();
    }

    public Rigidbody GetRigidbody()
    {
        return this.rb;
    }
}
