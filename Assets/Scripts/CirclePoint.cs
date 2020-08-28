using System;
using UnityEngine;

public class CirclePoint
{
    public Vector2 origin = Vector2.zero;
    public Vector2 point = Vector2.zero;
    public bool isEndPoint = false;
    public CirclePoint EndPointNeighbor;

    public CirclePoint(Vector2 _origin, Vector2 _point, bool _isEndpoint = false)
    {
        origin = _origin;
        point = _point;
        isEndPoint = _isEndpoint;
    }

    public CirclePoint()
    {
        origin = Vector2.zero;
        point = Vector2.zero;
        isEndPoint = false;
        EndPointNeighbor = null;
    }
}
