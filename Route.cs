using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Route {

    private string name;
    private Vector3[] waypoints;
    public float Length { get; set; } // Example: total length of the path

    public Route(string name, Vector3[] waypoints) {

        this.name = name;
        this.waypoints = waypoints;
        Length = CalculateLength();
    }

    public string Name {
        get { 
            return name; 
        }
        private set {
            name = value;
        }
    }

    public Vector3[] Waypoints {   
        get {
            return waypoints;
        }
        private set {
            waypoints = value;
        }
    }

    private float CalculateLength() {

        // Example logic to calculate path length
        float totalLength = 0f;
        for (int i = 0; i < waypoints.Length - 1; i++) {
            
            totalLength += Vector3.Distance(waypoints[i], waypoints[i + 1]);
        }
        return totalLength;
    }
}