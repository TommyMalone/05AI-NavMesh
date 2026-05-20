using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    private List<Edge> _edges = new List<Edge>();
    private List<Node> _nodes = new List<Node>();
    private List<Node> _pathList = new List<Node>();

    public Graph()
    {
        
    }

    public List<Node> GetPathList()
    {
        return _pathList;
    }

    public void AddNode(GameObject id)
    {
        Node node = new Node(id);
        _nodes.Add(node);
    }

    public void AddEdge(GameObject fromNode, GameObject toNode, Link.direction direction = Link.direction.BI)
    {
        Node from = FindNode(fromNode);
        Node to = FindNode(toNode);
        
        if(from != null && to != null)
        {
            Edge edge = new Edge(from, to);
            _edges.Add(edge);
            from.edges.Add(edge);
            if(direction == Link.direction.BI)
            {
                Edge reverseEdge = new Edge(to, from);
                _edges.Add(reverseEdge);
                to.edges.Add(reverseEdge);
            }
        }
    }

    private Node FindNode(GameObject id)
    {
        foreach (Node node in _nodes)
        {
            if(node.GetId() == id)
            {
                return node;
            }
        }
        return null;
    }

    public bool AStar(GameObject startId, GameObject endId)
    {
        if (startId != endId)
        {
            Node start = FindNode(startId);
            Node end = FindNode(endId);

            if (start != null && end != null)
            {
                List<Node> open = new List<Node>();
                List<Node> closed = new List<Node>();
                float tentativeGScore = 0;
                bool tentativeIsBetter = false;

                start.g = 0;
                start.h = DistanceSquared(start, end);
                start.f = start.g + start.h;

                open.Add(start);
                while (open.Count > 0)
                {
                    int indexOfLowestF = FindIndexOfLowestF(open);
                    Node thisNode = open[indexOfLowestF];
                    if (thisNode.GetId() == endId)
                    {
                        ReconstructPath(start, end);
                        return true;
                    }

                    open.RemoveAt(indexOfLowestF);
                    closed.Add(thisNode);
                    Node neighbor;
                    foreach (Edge edge in thisNode.edges)
                    {
                        neighbor = edge.endNode;
                        if (closed.IndexOf(neighbor) <= -1)
                        {
                            tentativeGScore = thisNode.g + DistanceSquared(thisNode, neighbor);
                            if (open.IndexOf(neighbor) == -1)
                            {
                                open.Add(neighbor);
                                tentativeIsBetter = true;
                            }
                            else if (tentativeGScore < neighbor.g)
                            {
                                tentativeIsBetter = true;
                            }
                            else
                            {
                                tentativeIsBetter = false;
                            }

                            if (tentativeIsBetter)
                            {
                                neighbor.cameFrom = thisNode;
                                neighbor.g = tentativeGScore;
                                neighbor.h = DistanceSquared(neighbor, end);
                                neighbor.f = neighbor.g + neighbor.h;
                            }
                        }
                    }
                }
            }
        }
        _pathList.Clear();
        return false;
    }

    private void ReconstructPath(Node startId, Node endId)
    {
        _pathList.Clear();
        _pathList.Add(endId);
        
        Node previousNode =endId.cameFrom;
        while (previousNode != startId && previousNode != null)
        {
            _pathList.Insert(0,previousNode);
            previousNode = previousNode.cameFrom;
        }
        _pathList.Insert(0,startId);
    }

    float DistanceSquared(Node a, Node b)
    {
        return Vector3.SqrMagnitude(a.GetId().transform.position - b.GetId().transform.position);
    }
    
    int FindIndexOfLowestF(List<Node> nodes)
    {
        float lowestF = 0;
        int count = 0;
        int iteratorCount = 0;

        lowestF = nodes[0].f;

        for (int index = 1; index < nodes.Count; index++)
        {
            if (nodes[index].f < lowestF)
            {
                lowestF = nodes[index].f;
                iteratorCount = count;

            }
        }

        return iteratorCount;
    }
}
