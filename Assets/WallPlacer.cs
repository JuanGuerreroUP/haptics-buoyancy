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
    

    void Awake()
    {
        PlaneCornerFinder cornerFinder = GetComponent<PlaneCornerFinder>();
        Vector3[] corners = cornerFinder.GetCorners();
        float x = cornerFinder.GetWidth()/2;
        float z = cornerFinder.GetDepth()/2;
        float y = corners[0].y;

        UpdateScale(leftWall, x, 2);
        UpdateScale(rightWall, x, 2);
        UpdateScale(northWall, z, 2);
        UpdateScale(southWall, z, 2);

        UpdatePosition(northWall, corners[1], x, y, 0);
        UpdatePosition(southWall, corners[3], x, y, 0);
        UpdatePosition(leftWall,  corners[3], 0, y, z);
        UpdatePosition(rightWall, corners[2], 0, y, z);
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
}
