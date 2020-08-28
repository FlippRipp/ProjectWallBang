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
                if (Vector2.Distance(debugPoints[i].origin, debugPoints[j].origin) < radius * 2)
                {
                    if (debugPoints[i].origin != debugPoints[j].origin)
                    {
                        if (debugPoints[i].isEndPoint && debugPoints[j].isEndPoint)
                        {
                            if (Vector2.Distance(debugPoints[i].point, debugPoints[j].point) <
                                distanceBetweenPointsOnCircle * 1.1f)
                            {
                                Line line = new Line(debugPoints[i].point + (Vector2) transform.position,
                                    debugPoints[j].point + (Vector2) transform.position);
                                lines.Add(line);
                            }
                        }
                    }
                    else if (Vector2.Distance(debugPoints[i].point, debugPoints[j].point) < distanceBetweenPointsOnCircle)
                    {
                        Line line = new Line(debugPoints[i].point + (Vector2)transform.position, debugPoints[j].point + (Vector2)transform.position);
                        lines.Add(line);
                    }
                }
            }

        }
    }

    private void CreateLine2()
    {
        for (int i = 0; i < debugPoints.Count; i++)
        {
            if (debugPoints[i].isEndPoint)
            {
                
            }
            else
            {
                
            }
        }
    }

    private void ConfigureEndpoints(CirclePoint currentPoint)
    {
        for (int j = 0; j < debugPoints.Count; j++)
        {
            if ( debugPoints[j].isEndPoint && currentPoint.EndPointNeighbor != null)
            {
                if (currentPoint.EndPointNeighbor.origin != debugPoints[j].origin)
                {
                    
                }
            }
            else if(debugPoints[j].isEndPoint)
            {
                currentPoint.EndPointNeighbor = 
            }
            else
            {
                
            }
        }

    }

    private void Update()
    {
        distanceBetweenPointsOnCircle = radius * 2 * Mathf.PI / pointsPerCircle * 1.3f;
        debugPoints.Clear();
        for (int i = 0; i < points.Count; i++)
        {
            CreateDebugPoints(points[i], i);
        }
        lines.Clear();
        CreateLines();

    }

    private void CreateDebugPoints(Vector2 pos, int currentCircle)
    {
        if(pointsPerCircle < Mathf.Epsilon) return;
        bool previusCircleDrawn = true;
        bool firstCirclePointDrawn = false;
        CirclePoint firstCirclePoint = new CirclePoint();
        for (float j = 0; j < 359; j += (float)360 / pointsPerCircle)
        {
            
            
            Vector2 radiusPoint = Quaternion.AngleAxis(j, Vector3.forward) * (radius * transform.up);
            if (!CheckIntersectionWithPoints(radiusPoint + pos, currentCircle))
            {
                CirclePoint circlepoint = new CirclePoint(points[currentCircle], radiusPoint + pos);

                if (j == 0)
                {
                    firstCirclePoint = circlepoint;
                    firstCirclePointDrawn = true;
                }

                if (j + 360 / pointsPerCircle > 359)
                {
                    circlepoint.isEndPoint = !firstCirclePointDrawn;
                }
                
                if (!previusCircleDrawn)
                {
                    circlepoint.isEndPoint = true;
                }
                debugPoints.Add(circlepoint);

                previusCircleDrawn = true;
            }
            else
            {
                if (j + 360 / pointsPerCircle > 359)
                {
                    firstCirclePoint.isEndPoint = true;
                }

                if (previusCircleDrawn)
                {
                    if(debugPoints.Count > 0)
                    debugPoints[debugPoints.Count - 1].isEndPoint = true;
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
                Gizmos.DrawSphere(debugPoints[i].point + (Vector2)transform.position, 0.1f);
            }
            Gizmos.color = Color.red;
            for (int i = 0; i < points.Count; i++)
            {
                Gizmos.DrawSphere((Vector3)points[i] + transform.position, 0.1f);
            }
            Gizmos.color = Color.black;
            for (int i = 0; i < lines.Count; i++)
            {
                Gizmos.DrawLine(lines[i].point1, lines[i].point2);
            }

    }

}
