using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformHelper : MonoBehaviour
{
    [HideInInspector]
    public Vector3 scale;
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Quaternion rotation;
    // Start is called before the first frame update

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

    // Update is called once per frame
    void FixedUpdate()
    {
        this.UpdateTransform();
    }
}
