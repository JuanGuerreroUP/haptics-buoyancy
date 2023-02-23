using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsHelper : MonoBehaviour
{
    private Rigidbody rb;
    public float drag;
    public Vector3 velocity;
    public Vector3 position;
    public float mass;
    // Start is called before the first frame update
    void Awake()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.drag = this.rb.drag;
        this.velocity = this.rb.velocity;
        this.position = this.rb.position;
        this.mass = this.rb.mass;
    }

    public Rigidbody GetRigidbody()
    {
        return this.rb;
    }
}
