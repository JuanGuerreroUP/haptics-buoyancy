using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsHelper : MonoBehaviour
{
    private Rigidbody rb;
    private float drag;
    private Vector3 velocity;
    private Vector3 position;
    private float mass;


    public float Drag
    {
        get { return this.drag;  }
    }
    public Vector3 Velocity
    {
        get { return this.velocity; }
    }
    public Vector3 Position
    {
        get { return this.position; }
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
        this.drag = this.rb.drag;
        this.velocity = this.rb.velocity;
        this.position = this.rb.position;
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