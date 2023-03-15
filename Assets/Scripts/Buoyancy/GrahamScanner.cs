using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GrahamScanner
{
    public static List<Vector3> SortPoints(List<Vector3> vertices)
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

    private static int CompareAngles(Vector3 lowest, Vector3 v1, Vector3 v2){
        Vector3 cross = Vector3.Cross(v1 - lowest, v2 - lowest);
        if (cross.y > 0) {
            return -1;
        }
        else if (cross.y < 0) {
            return 1;
        }
        else {
            return 0;
        }
    }

    private static bool IsRightTurn(Vector3 v1, Vector3 v2, Vector3 v3) {
        Vector3 cross = Vector3.Cross(v2 - v1, v3 - v1);
        return cross.y < 0;
    }

    private static int GetLowestIndex(List<Vector3> vertices){
        int idx = 0;
        for(int i = 1; i < vertices.Count; i++) {
            if (vertices[i].y < vertices[idx].y || (vertices[i].y == vertices[idx].y && vertices[i].x < vertices[idx].x)) {
                idx = i;
            }
        }
        return idx;
    }
}
