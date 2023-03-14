using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GrahamScanner
{
    public static List<Vector3> SortPoints(Vector3[] vertices)
    {
        List<Vector3> sorted = new List<Vector3>();
        int lowestIndex = GetLowestIndex(vertices);
        List<Vector3> angleSorted = new List<Vector3>(vertices);
        angleSorted.RemoveAt(lowestIndex);
        angleSorted.Sort((p1, p2) => CompareAngles(vertices[lowestIndex], p1, p2));

        sorted.Add(vertices[lowestIndex]);
        sorted.Add(angleSorted[0]);
        sorted.Add(angleSorted[1]);

        for (int i = 2; i < angleSorted.Count; i++) {
            Vector3 current = angleSorted[i];
            while (sorted.Count >= 2 && IsRightTurn(sorted[sorted.Count - 2], sorted[sorted.Count - 1], current)) {
                sorted.RemoveAt(sorted.Count - 1);
            }
            sorted.Add(current);
        }
        return sorted;
    }

    private static int CompareAngles(Vector3 lowest, Vector3 v1, Vector3 v2) {
        float angle1 = Mathf.Atan2(v1.y - lowest.y, v1.x - lowest.x);
        float angle2 = Mathf.Atan2(v2.y - lowest.y, v2.x - lowest.x);

        if (angle1 < angle2) {
            return -1;
        }
        else if (angle1 > angle2) {
            return 1;
        }
        else {
            return 0;
        }
    }

    private static bool IsRightTurn(Vector3 p1, Vector3 p2, Vector3 p3) {
        return ((p2.x - p1.x) * (p3.y - p1.y) - (p2.y - p1.y) * (p3.x - p1.x)) < 0;
    }

    private static int GetLowestIndex(Vector3[] vertices){
        int idx = 0;
        for(int i = 1; i < vertices.Length; i++) {
            if (vertices[i].y < vertices[idx].y || (vertices[i].y == vertices[idx].y && vertices[i].x < vertices[idx].x)) {
                idx = i;
            }
        }
        return idx;
    }
}
