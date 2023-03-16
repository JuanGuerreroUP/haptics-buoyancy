using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaneCornerFinder))]
public class BuoyPlacer : MonoBehaviour
{
    public GameObject buoyPref;
    public Transform buoyDaddy;
    private int buoysNum;
    public Vector3[] buoyBoxCorners = new Vector3[4];

    void Start()
    {
        PlaneCornerFinder cornerFinder = GetComponent<PlaneCornerFinder>();
        Vector3[] corners = cornerFinder.GetCorners();

        BuoyBoxCorners(corners);
        float buoySquareWidth = Mathf.Abs(buoyBoxCorners[0].x - buoyBoxCorners[1].x);
        float buoySquareDepth = Mathf.Abs(buoyBoxCorners[0].z - buoyBoxCorners[2].z);

        SpawnBuoysWidth(5, buoyBoxCorners[0], buoySquareWidth);
        SpawnBuoysWidth(5, buoyBoxCorners[2], buoySquareWidth);
        SpawnBuoysDepth(5, buoyBoxCorners[0], buoySquareDepth);
    }
    private void SpawnBuoysWidth(int divisions, Vector3 buoyCorner, float x)
    {
        float buoysSeparate = (x) / divisions;
        for (int i = 0; i <= divisions; i++)
        {
            Instantiate(buoyPref, new Vector3(buoyCorner.x - (buoysSeparate * i * Mathf.Sign(buoyCorner.x)), buoyCorner.y, buoyCorner.z), Quaternion.identity, buoyDaddy);
        }
    }

    private void SpawnBuoysDepth(int divisions, Vector3 buoyCorner, float z)
    {
        float buoysSeparate = (z) / divisions;
        for (int i = 1; i < divisions; i++)
        {
            Instantiate(buoyPref, new Vector3(buoyCorner.x, buoyCorner.y, buoyCorner.z - (buoysSeparate * i)), Quaternion.identity, buoyDaddy);
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
