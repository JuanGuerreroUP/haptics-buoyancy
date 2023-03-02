using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;


[RequireComponent(typeof(MeshFilter))]
public class PlaneCornerFinder : MonoBehaviour
{
    private Vector3[] corners = null;

    private void FindCorners(){
        if (corners != null) {
            return;
        }
        MeshFilter mesh = GetComponent<MeshFilter>();
        corners = new Vector3[4];
        Vector3[] vertices = mesh.sharedMesh.vertices;
        corners[0] = this.transform.TransformPoint(vertices[0]);
        corners[1] = this.transform.TransformPoint(vertices[10]);
        corners[2] = this.transform.TransformPoint(vertices[110]);
        corners[3] = this.transform.TransformPoint(vertices[120]);
    }
    public Vector3[] GetCorners()
    {
        FindCorners();
        return corners;
    }

    public float GetWidth()
    {
        FindCorners();
        return Mathf.Abs(corners[0].x - corners[1].x);
    }

    public float GetDepth()
    {
        FindCorners();
        return Mathf.Abs(corners[0].z - corners[2].z);
    }
}
