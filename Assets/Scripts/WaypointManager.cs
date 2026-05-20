using UnityEngine;

[System.Serializable]
public struct Link
{
    public enum direction
    {
        BI = 0,
        UNI = 1,
        
    };

    public GameObject node1;
    public GameObject node2;
    public direction linkDirection;
}

public class WaypointManager : MonoBehaviour
{
    public GameObject[] waypoints;

    public Link[] links;
    public Graph graph = new Graph();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (waypoints.Length > 0)
        {
            foreach (GameObject waypoint in waypoints)
            {
                graph.AddNode(waypoint);
            }

            foreach (Link link in links)
            {
                graph.AddEdge(link.node1, link.node2, link.linkDirection);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
