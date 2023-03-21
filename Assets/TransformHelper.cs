using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformHelper : MonoBehaviour
{
    private Vector3 previousPosition;
    public Vector3 Scale
    {
        get; private set;
    }
    public Vector3 Position
    {
        get; private set;
    }
    public Quaternion Rotation
    {
        get; private set;
    }

    public Vector3 Velocity
    {
        get; private set;
    }

    private void UpdateTransform()
    {
        this.Position = transform.position;
        this.Rotation = transform.rotation;
        this.Scale = transform.lossyScale;
    }
    void Awake()
    {
        this.UpdateTransform();
        this.Velocity = Vector3.zero;
    }

    void Update()
    {
        this.Velocity = (transform.position - this.Position) / Time.deltaTime;
        this.UpdateTransform();

    }
}
