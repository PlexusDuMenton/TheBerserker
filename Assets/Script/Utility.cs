using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility {

    public static Vector3 GetMouseWorldPosition(Vector3 Pos)
    {
        Plane playerPlane = new Plane(Vector3.up, Pos);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitdist;
        if (playerPlane.Raycast(ray, out hitdist))
        {
            // Get the point along the ray that hits the calculated distance.
            return ray.GetPoint(hitdist);
        }
        return Vector3.zero;

    }
}
