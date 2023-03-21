using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter))]
public class RotatedCubeHelper : MonoBehaviour
{
    private Vector3[] vertices;
    private Vector3[] worldVertices;

    private GameObject[] spheres;
    private MeshFilter[] triangles;
    private float volume;

    public float waterLevel;

    public bool debug;
    // Start is called before the first frame update
    void Awake()
    {
        Bounds bounds = GetComponent<MeshFilter>().mesh.bounds;
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;

        this.vertices = new Vector3[8];
        this.worldVertices = new Vector3[8];

        this.vertices[0] = min;
        this.vertices[1] = new Vector3(max.x, min.y, min.z);
        this.vertices[2] = new Vector3(max.x, min.y, max.z);
        this.vertices[3] = new Vector3(min.x, min.y, max.z);

        this.vertices[4] = new Vector3(min.x, max.y, min.z);
        this.vertices[5] = new Vector3(max.x, max.y, min.z);
        this.vertices[6] = max;
        this.vertices[7] = new Vector3(min.x, max.y, max.z);
        this.volume = this.transform.localScale.x * this.transform.localScale.y * this.transform.localScale.z;

    }

    // Update is called once per frame
    void FixedUpdate() {
        GetWorldVertices();
        if(GetMinVertexAt(1).y > this.waterLevel){
            this.volume = 0;
            Plot(null);
            return;
        }

        TruncatedCube truncatedCube;
        if (this.waterLevel < this.transform.position.y) {
            truncatedCube = GetUnderwaterMesh(this.waterLevel);
            this.volume = truncatedCube.GetVolume();
        }
        else {
            truncatedCube = GetUnderwaterMesh(this.transform.position.y);
            this.volume = truncatedCube.GetVolume() + truncatedCube.GetPrismVolume(this.waterLevel);
        }
        Debug.Log("Volume: " + this.volume);
        Plot(truncatedCube);
    }

    private void Plot(TruncatedCube truncatedCube){
        if (debug && spheres == null) {
            CreateSpheres();
        }
        if (!debug || truncatedCube == null) {
            HideSpheres();
            HideTriangles();
        }
        else {
            DrawVetices(truncatedCube);
            DrawTriangles(truncatedCube);
        }
    }

    private void GetWorldVertices() {
        for(int i = 0; i < 8; i++) {
            this.worldVertices[i] = this.transform.TransformPoint(this.vertices[i]);
        }
    }

    public float GetVolume(){
        return this.volume;
    }

    private Vector3 GetMinVertexAt(int dim){
        Vector3 minVertex = this.worldVertices[0];
        for (int i = 1; i < 8; i++)
        {
            if (this.worldVertices[i][dim] < minVertex[dim]){
                minVertex = this.worldVertices[i];
            }
        }
        return minVertex;

    }
    

    private TruncatedCube GetUnderwaterMesh(float xzPlane){
        TruncatedCube truncatedCube = new(xzPlane, this.worldVertices, this.transform.position);
        truncatedCube.CalcFace(0, 1, 2, 3);
        truncatedCube.CalcFace(0, 1, 5, 4);
        
        truncatedCube.CalcFace(4, 5, 6, 7);
        truncatedCube.CalcFace(3, 2, 6, 7);

        truncatedCube.CalcFace(2, 1, 5, 6);
        truncatedCube.CalcFace(3, 0, 4, 7);

        return truncatedCube;
    }

    private void CreateSpheres(){
        spheres = new GameObject[10];
        for (int i = 0; i < spheres.Length; i++) {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = Vector3.one * 0.05f;
            sphere.GetComponent<Renderer>().material.color = new Color(0, 0, 1);
            Destroy(sphere.GetComponent<SphereCollider>());
            spheres[i] = sphere;
        }
    }

    private void HideSpheres() {
        if(spheres == null){
            return;
        }
        foreach (GameObject obj in spheres) {
            obj.SetActive(false);
        }
    }

    private void HideTriangles() {
        if (triangles == null) {
            return;
        }
        foreach (MeshFilter obj in triangles) {
            obj.gameObject.SetActive(false);
        }
    }

    private void CreateTriangles()
    {
        
        triangles = new MeshFilter[20];
        for (int i = 0; i < triangles.Length; i++)
        {
            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.localScale = Vector3.one * 0.1f;
            float hue = UnityEngine.Random.Range(0f, 1f);
            plane.GetComponent<Renderer>().material.color = Color.HSVToRGB(hue, 1, 0.5f + (i * 0.05f));
            Destroy(plane.GetComponent<MeshCollider>());
            triangles[i] = plane.GetComponent<MeshFilter>();
            triangles[i].mesh = new Mesh();
        }
    }

    void DrawVetices(TruncatedCube truncatedCube)
    {
        if(spheres == null){
            CreateSpheres();
        }
        Vector3[] cubeVertices = truncatedCube.GetVertices();
        for (int i = 0; i < cubeVertices.Length; i++) {
            spheres[i].transform.position = cubeVertices[i];
            spheres[i].SetActive(true);
        }
        for(int i = cubeVertices.Length; i < spheres.Length; i++) {
            spheres[i].SetActive(false);
        }
    }

    private Vector3[] GetTrianglesVertices(Transform mesh, Vector3[] face)
    {
        int[] order = new int[] { 0, 1, 2, 2, 1, 0 };
        Vector3[] vertices = new Vector3[order.Length];
        for (int i = 0; i < order.Length; i++){
            vertices[i] = mesh.InverseTransformPoint(face[order[i]]);
        }
        return vertices;
    }

    void DrawTriangles(TruncatedCube truncatedCube){
        if(triangles == null) {
            CreateTriangles();
        }
        List<Vector3[]> faces = truncatedCube.GetTriangles();
        for(int i = 0; i < faces.Count; i++) {
            triangles[i].mesh.vertices = GetTrianglesVertices(triangles[i].transform, faces[i]);
            triangles[i].mesh.triangles = new int[] { 0, 1, 2, 3, 4, 5 };
            triangles[i].gameObject.SetActive(true);
        }
        for (int i = faces.Count; i < triangles.Length; i++){
            triangles[i].gameObject.SetActive(false);
        }
    }
}
