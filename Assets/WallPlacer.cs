using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaneCornerFinder))]
public class WallPlacer : MonoBehaviour
{
    public Transform northWall;
    public Transform southWall;
    public Transform leftWall;
    public Transform rightWall;
    public Transform floor;
    public Liquid liquid;

    public GameObject buoyPref;
    private int buoysNum;
    public Vector3[] buoyBoxCorners = new Vector3[4];



    void Start()
    {
        PlaneCornerFinder cornerFinder = GetComponent<PlaneCornerFinder>();
        Vector3[] corners = cornerFinder.GetCorners();
        float x = cornerFinder.GetWidth();
        float z = cornerFinder.GetDepth();
        float y = corners[0].y;

        UpdateScale(leftWall, x, 2);
        UpdateScale(rightWall, x, 2);
        UpdateScale(northWall, z, 2);
        UpdateScale(southWall, z, 2);
        UpdateScale(floor, x, 0);
        UpdateScale(floor, z, 2);

        x /=2;
        z /=2;

        UpdatePosition(northWall, corners[1], x, y, 0);
        UpdatePosition(southWall, corners[3], x, y, 0);
        UpdatePosition(leftWall,  corners[3], 0, y, z);
        UpdatePosition(rightWall, corners[2], 0, y, z);
        Vector3 center = cornerFinder.GetCenter();
        floor.position = new Vector3(center.x, liquid.GetFloorLevel(), center.z);


        BuoyBoxCorners(corners);
        float buoySquareWidth = Mathf.Abs(buoyBoxCorners[0].x - buoyBoxCorners[1].x);
        float buoySquareDepth = Mathf.Abs(buoyBoxCorners[0].z - buoyBoxCorners[2].z);

        SpawnBuoys(5, buoyBoxCorners[0], buoySquareWidth);
        SpawnBuoys(5, buoyBoxCorners[2], buoySquareWidth);

    }

    private void UpdateScale(Transform wall, float scale, int index)
    {
        Vector3 aux = wall.localScale;
        aux[index] = scale;
        wall.localScale = aux;
    }

    private void UpdatePosition(Transform wall, Vector3 corner, float x, float y, float z) {
        wall.position = new Vector3(corner.x + x, y, corner.z + z);
    }

    private void SpawnBuoys(int divisions, Vector3 buoyCorner, float x)
    {
        float buoysSeparate = (x) / divisions;
        for (int i = 0; i <= divisions; i++)
        {
            Instantiate(buoyPref, new Vector3(buoyCorner.x - (buoysSeparate * i * Mathf.Sign(buoyCorner.x)), buoyCorner.y, buoyCorner.z), Quaternion.identity);
        }
    }

    private void BuoyBoxCorners(Vector3[] corners)
    {
        float buoyCornerValue = Mathf.Abs(corners[0].x) - buoyPref.GetComponent<SphereCollider>().radius * 2;
        for (int i = 0; i < corners.Length; i++)
        {
            buoyBoxCorners[i] = new Vector3(buoyCornerValue * Mathf.Sign(corners[i].x), corners[i].y, buoyCornerValue * Mathf.Sign(corners[i].z));
        }
    }
}
