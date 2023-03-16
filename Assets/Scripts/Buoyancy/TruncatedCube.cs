using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TruncatedCube {

    private readonly float xzPlane;
    private readonly List<Vector3> cubeVertices;
    private readonly Vector3 center;
    private readonly List<List<Vector3>> faces;
    private readonly HashSet<Vector3> newFace;
    private readonly Vector3[] normals;

    public TruncatedCube(float xzPlane, Vector3[] vertices, Vector3 center) {
        this.cubeVertices = new(vertices);
        this.normals = new Vector3[6];
        this.faces = new();
        this.newFace = new();
        this.xzPlane = xzPlane;
        this.center = center;
    }

    public List<List<Vector3>> Faces {
        get {
            return faces;
        }
    }

    public HashSet<Vector3> NewFace {
        get {
            return newFace;
        }
    }

    public Vector3[] GetVertices() {
        HashSet<Vector3> vertices = new ();
        foreach (List<Vector3> vector in this.Faces) {
            vertices.UnionWith(vector);
        }
        vertices.UnionWith(this.NewFace);
        return vertices.ToArray();
    }

    private Vector3 GetIntersection(Vector3 a, Vector3 b) {
        float x = a.x + ((this.xzPlane - a.y) / (b.y - a.y)) * (b.x - a.x);
        float z = a.z + ((this.xzPlane - a.y) / (b.y - a.y)) * (b.z - a.z);
        return new(x, this.xzPlane, z);
    }

    [Obsolete]
    private void SetNextNormal(int v1, int v2, int v3, int v4) {
        //How can I know if a point is in the same plane of a plain defined by three points?
        normals[this.faces.Count] = Vector3.Cross(
            this.cubeVertices[v2] - this.cubeVertices[v1],
            this.cubeVertices[v4] - this.cubeVertices[v1]
       ).normalized;
    }

    public void CalcFace(int v1, int v2, int v3, int v4) {
        //SetNextNormal(v1, v2, v3, v4);
        List<Vector3> updatedVertices = new();
        Vector3 vertex;
        int[] indexes = { v4, v1, v2, v3, v4, v1 };
        for (int i = 1; i < 5; i++)
        {
            vertex = this.cubeVertices[indexes[i]];
            if (vertex.y <= this.xzPlane)
            {
                updatedVertices.Add(vertex);
                continue;
            }
            AddNeighborIntesection(vertex, this.cubeVertices[indexes[i - 1]], updatedVertices);
            AddNeighborIntesection(vertex, this.cubeVertices[indexes[i + 1]], updatedVertices);

        }
        if (updatedVertices.Count > 0) {
            this.faces.Add(updatedVertices);
        }
    }

    private void AddNeighborIntesection(Vector3 vertex, Vector3 neighbor, List<Vector3> updatedVertices)
    {
        if (vertex.y == neighbor.y) {
            return;
        }
        if (neighbor.y <= this.xzPlane){
            Vector3 newVertex = GetIntersection(vertex, neighbor);
            updatedVertices.Add(newVertex);
            this.newFace.Add(newVertex);
        }
    }

    private void PolygonToTriangles(List<Vector3> polygon, List<Vector3[]> outlist){
        int nTriangles = polygon.Count - 2;
        for (int i = 1; i <= nTriangles; i++) {
            outlist.Add(new Vector3[] { polygon[0], polygon[i], polygon[i+1] });
        }
    }

    private void FaceToTriangles(List<Vector3> vertices, List<Vector3[]> outlist, bool sorted) {
        if(vertices.Count <= 2) {
            Debug.LogWarning("Recieved a wrong face with " + vertices.Count + "vertices");
            return;
        }
        if (vertices.Count == 3) {
            outlist.Add(vertices.ToArray());
            return;
        }
        if (!sorted) {
            vertices = GrahamScanner.SortPoints(vertices);
        }
        PolygonToTriangles(vertices, outlist);
    }

    private bool GetNewFaceTriangles(List<Vector3[]> outList){
        if (this.newFace.Count > 0) {
            FaceToTriangles(this.newFace.ToList(), outList, false);
            return true;
        }
        return false;
    }

    public List<Vector3[]> GetTriangles(){
        List<Vector3[]> triangles = new();
        foreach (List<Vector3> face in faces) {
            FaceToTriangles(face, triangles, true);
        }
        GetNewFaceTriangles(triangles);
        return triangles;
    }

    private Vector3 GetIntersectionPoint(){
        return new Vector3(this.center.x, this.xzPlane, this.center.z);
    }

    public float GetVolume()
    {
        List<Vector3[]> triangles = GetTriangles();
        Vector3 intersectionPoint = GetIntersectionPoint();
        float totalVolume = 0f;


        foreach (Vector3[] triangle in triangles) {
            Vector3 normal = Vector3.Cross(triangle[1] - triangle[0], triangle[2] - triangle[0]);

            Vector3 toKnownPoint = intersectionPoint - triangle[0];
            if (Vector3.Dot(normal, toKnownPoint) < 0) {
                Vector3 temp = triangle[1];
                triangle[1] = triangle[2];
                triangle[2] = temp;
            }
            totalVolume += GetTetrahedronVolume(intersectionPoint, triangle, normal);
        }

        return totalVolume;
    }

    public float GetPrismVolume(float top) {
        List<Vector3[]> triangles = new();
        if (!GetNewFaceTriangles(triangles)){
            return 0f;
        }
        float height = top - this.xzPlane;
        float totalVolume = 0f;
        foreach (Vector3[] triangle in triangles) {
            totalVolume += GetTriangleArea(triangle);
        }
        return totalVolume * height;
    }

    private float GetTetrahedronVolume(Vector3 intersectionPoint, Vector3[] triangle, Vector3 normal)
    {
        Vector3 base1 = triangle[1] - triangle[0];
        Vector3 base2 = triangle[2] - triangle[0];
        Vector3 heightVec = intersectionPoint - triangle[0];
        float baseArea = 0.5f * Vector3.Cross(base1, base2).magnitude;
        float height = Mathf.Abs(Vector3.Dot(heightVec, normal.normalized));
        float volume = (1f / 3f) * baseArea * height;
        if (volume < 0f) {
            Debug.LogWarning("Got a negative volume: " + volume);
        }
        return volume;
    }

    private float GetTriangleArea(Vector3[] triangle){
        float b = Vector3.Distance(triangle[0], triangle[1]);
        Vector3 half = (triangle[0] + triangle[1]) * 0.5f;
        float h = Vector3.Distance(half, triangle[2]);
        return b * h / 2;
    }
}