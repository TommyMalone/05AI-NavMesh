using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Edge> edges = new List<Edge>();
    public Node path = null;
    private GameObject _id;


    public float f, g, h;
    public Node cameFrom;
    
    public Node(GameObject id)
    {
        _id = id;
    }
    
    public GameObject GetId()
    {
        return _id;
    }

}
