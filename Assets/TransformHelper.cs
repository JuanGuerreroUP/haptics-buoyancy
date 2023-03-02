using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformHelper : MonoBehaviour
{
    private Vector3 scale;
    private Vector3 position;
    private Quaternion rotation;

    public Vector3 Scale
    {
        get { return this.scale; }
    }
    public Vector3 Position
    {
        get { return this.position; }
    }
    public Quaternion Rotation
    {
        get { return this.rotation; }
    }

    private void UpdateTransform()
    {
        this.position = transform.position;
        this.rotation = transform.rotation;
        this.scale = transform.localScale;
    }
    void Awake()
    {
        this.UpdateTransform();
    }

    void Update()
    {
        this.UpdateTransform();
    }
}
