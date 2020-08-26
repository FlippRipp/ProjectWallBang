using System;
using UnityEngine;
using  System.Collections.Generic;

public class PointsOnAreaBorder : MonoBehaviour
{
    [SerializeField]private List<Vector2> points = new List<Vector2>();
    [SerializeField] private List<CirclePoint> debugPoints = new List<CirclePoint>();

    [SerializeField] private float radius = 20;
    [SerializeField] private float pointsPerCircle = 20;
    private List<Line> lines = new List<Line>();
    private float distanceBetweenPointsOnCircle;
    
    [Serializable]
    struct Line
    {
        public Vector2 Point1;
        public Vector2 Point2;
    }
    
    [Serializable]
    struct CirclePoint
    {
        public Vector2 Origin;
        public Vector2 Point;
        public bool isEndPoint;
    }

    private void Awake()
    {
        distanceBetweenPointsOnCircle = radius * 2 * Mathf.PI / pointsPerCircle * 1.3f;
        //debugPoints.Clear();
        for (int i = 0; i < points.Count; i++)
        {
            CreateDebugPoints(points[i], i);
        }
        CreateLines();
        
    }

    private void CreateLines()
    {
        for (int i = 0; i < debugPoints.Count; i++)
        {
            for (int j = 0; j < debugPoints.Count; j++)
            {
                if (Vector2.Distance(debugPoints[i].Origin, debugPoints[j].Origin) < radius * 2)
                {
                    if (Vector2.Distance(debugPoints[i].Point, debugPoints[j].Point) < distanceBetweenPointsOnCircle)
                    {
                        Line line = new Line();
                        line.Point1 = debugPoints[i].Point + (Vector2)transform.position;
                        line.Point2 = debugPoints[j].Point + (Vector2)transform.position;
                        lines.Add(line);
                    }
                }
            }

        }
    }

    private void Update()
    {

    }

    private void CreateDebugPoints(Vector2 pos, int currentCircle)
    {
        if(pointsPerCircle < Mathf.Epsilon) return;
        bool previusCircleDrawn = true;
        for (float j = 0; j < 359; j += (float)360 / pointsPerCircle)
        {
            Vector2 radiusPoint = Quaternion.AngleAxis(j, Vector3.forward) * (radius * transform.up);
            if (!CheckIntersectionWithPoints(radiusPoint + pos, currentCircle))
            {
                CirclePoint circlepoint = new CirclePoint();
                circlepoint.Origin = points[currentCircle];
                circlepoint.Point = radiusPoint + pos;
                if (!previusCircleDrawn)
                {
                    circlepoint.isEndPoint = true;
                }
                debugPoints.Add(circlepoint);

                previusCircleDrawn = true;
            }
            else
            {
                if (previusCircleDrawn)
                {
                    debugPoints[1].isEndPoint = true;
                }
                previusCircleDrawn = false;
            }

        }

    }

    private bool CheckIntersectionWithPoints(Vector2 pos, int currentCircle)
    {
        for (int i = 0; i < points.Count; i++)
        {
            if(currentCircle == i) continue;
            if (Vector2.Distance(pos, points[i]) < radius)
            {
                return true;
            }
        }

        return false;
    }
    
    private void OnDrawGizmos()
    {
            for (int i = 0; i < debugPoints.Count; i++)
            {
                if (debugPoints[i].isEndPoint)
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.grey;
                }
                Gizmos.DrawSphere(debugPoints[i].Point + (Vector2)transform.position, 0.1f);
            }
            Gizmos.color = Color.red;
            for (int i = 0; i < points.Count; i++)
            {
                Gizmos.DrawSphere((Vector3)points[i] + transform.position, 0.1f);
            }
            Gizmos.color = Color.black;
            for (int i = 0; i < lines.Count; i++)
            {
                Gizmos.DrawLine(lines[i].Point1, lines[i].Point2);
            }

    }

}
