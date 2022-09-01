using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mathc {
    
    /// <summary>
    /// Calculates the average point from a set of 2D points. Returns Vector2.zero if given an empty array.
    /// </summary>
    public static Vector2 Average(Vector2[] points) {
        if(points.Length == 0) {
            return Vector2.zero;
        }
        float sumX = 0;
        float sumY = 0;
        int numPoints = points.Length;
        foreach(Vector2 point in points) {
            sumX += point.x;
            sumY += point.y;
        }
        return new Vector2(sumX / numPoints, sumY / numPoints);
    }

}
