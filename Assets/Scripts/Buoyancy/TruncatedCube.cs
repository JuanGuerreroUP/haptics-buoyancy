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
    private readonly List<List<Vector3>> faces;
    private readonly HashSet<Vector3> newFace;

    public TruncatedCube(float xzPlane, Vector3[] vertices) {
        this.cubeVertices = new(vertices);
        this.faces = new();
        this.newFace = new();
        this.xzPlane = xzPlane;
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

    public void CalcFace(int v1, int v2, int v3, int v4) {
        List<Vector3> updatedVertices = new();
        Vector3 vertex, nextVertex, newVertex;
        int[] indexes = { v1, v2, v3, v4, v1 };
        for (int i = 0; i < 4; i++) {
            vertex = this.cubeVertices[indexes[i]];
            if (vertex.y <= this.xzPlane) {
                updatedVertices.Add(vertex);
                continue;
            }
            nextVertex = this.cubeVertices[indexes[i + 1]];
            if (nextVertex.y > this.xzPlane) {
                int idx = i - 1;
                if(idx < 0){
                    idx = 3; //v4
                }
                nextVertex = this.cubeVertices[indexes[idx]];
                if(nextVertex.y > this.xzPlane){
                    continue;
                }
            }
            newVertex = GetIntersection(vertex, nextVertex);
            updatedVertices.Add(newVertex);
            this.newFace.Add(newVertex);
        }
        if (updatedVertices.Count > 0) {
            this.faces.Add(updatedVertices);
        }
    }

    private int GetDiagonalIndex(List<Vector3> square)
    {
        int far = 0;
        float maxDistance = 0;
        float temp;
        for (int i = 1; i < 4; i++) {
            temp = Vector3.Distance(square[0], square[i]);
            if (temp > maxDistance) {
                maxDistance = temp;
                far = i;
            }
        }
        return far;
    }

    private void SquareToTriangles(List<Vector3> square, List<Vector3[]> outlist, bool sorted) {
        int[] neighbors;
        int far;
        if (sorted) {
            neighbors = new int[] { 1, 3 };
            far = 2;
        }
        else {
            far = GetDiagonalIndex(square);
            HashSet<int> neighborsSet = new HashSet<int>(new int[] { 1, 2, 3 });
            neighborsSet.Remove(far);
            neighbors = neighborsSet.ToArray();
        }
        outlist.Add(new Vector3[] { square[0], square[neighbors[0]], square[far] });
        outlist.Add(new Vector3[] { square[0], square[neighbors[1]], square[far] });
    }

    private void PentagonToTriangles(List<Vector3> pentagon, List<Vector3[]> outlist){
        outlist.Add(new Vector3[] { pentagon[0], pentagon[1], pentagon[2] });
        outlist.Add(new Vector3[] { pentagon[0], pentagon[2], pentagon[4] });
        outlist.Add(new Vector3[] { pentagon[4], pentagon[2], pentagon[3] });
    }

    private void FaceToTriangles(List<Vector3> vertices, List<Vector3[]> outlist, bool sorted) {
        if (vertices.Count == 3) {
            outlist.Add(vertices.ToArray());
        }
        else if (vertices.Count == 4) {
            SquareToTriangles(vertices, outlist, sorted);
        }
        else if (vertices.Count == 5) {
            PentagonToTriangles(vertices, outlist);
        }
        else {
            Debug.LogWarning("Recieved a wrong face with " + vertices.Count + "vertices");
        }
    }

    public List<Vector3[]> GetTriangles(){
        List<Vector3[]> triangles = new();
        foreach (List<Vector3> face in faces) {
            FaceToTriangles(face, triangles, true);
        }
        if(this.newFace.Count > 0) {
            FaceToTriangles(this.newFace.ToList(), triangles, false);
        }
        return triangles;
    }



}