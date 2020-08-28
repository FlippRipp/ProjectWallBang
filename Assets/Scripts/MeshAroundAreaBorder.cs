using System;
using UnityEngine;
using  System.Collections.Generic;

public class MeshAroundAreaBorder : MonoBehaviour
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
        ConfigureEndpoint();
        

        foreach (CirclePoint circlePoint in debugPoints)
        {
            if (circlePoint.neighborPoints[0] != null)
            {
                Line line = new Line(circlePoint.point + (Vector2) transform.position, circlePoint.neighborPoints[0].point + (Vector2) transform.position);
                lines.Add(line);
            }
            else
            {
                Debug.LogWarning("Neighbors are null");
            }
        }
    }

    private void ConfigureEndpoint()
    {

        foreach (CirclePoint circlePoint in debugPoints)
        {
            if (!circlePoint.isEndPoint) continue;
            
                
            if (circlePoint.neighborPoints[0] == null)
            {
                CirclePoint closestPoint = FindOtherEndpoint(circlePoint);

                circlePoint.neighborPoints[0] = closestPoint;
            }
            if(circlePoint.neighborPoints[1] == null)
            {
                CirclePoint closestPoint = FindOtherEndpoint(circlePoint);

                circlePoint.neighborPoints[1] = closestPoint;
            }

        }
    }

    private CirclePoint FindClosestEndpoint(CirclePoint current, CirclePoint currentNeighbor = null)
    {
        CirclePoint closestPoint = null;
        float closestPointDist = float.PositiveInfinity;
        
        foreach (CirclePoint circlePoint in debugPoints)
        {
            if(!circlePoint.isEndPoint || circlePoint == current || circlePoint == currentNeighbor) continue;
            
            if(circlePoint.origin == current.origin) continue;

            //if (Vector2.Distance(current.origin, circlePoint.origin) > radius * 2) continue;
            
            if(currentNeighbor != null)
                if (circlePoint.origin == currentNeighbor.origin) continue;
            
            float dist = Vector2.Distance(current.point, circlePoint.point);
            
            if (!(dist < closestPointDist)) continue;
            
            closestPoint = circlePoint;
            closestPointDist = dist;
        }
        return closestPoint;
    }
    
    private CirclePoint FindOtherEndpoint(CirclePoint current)
    {
        List<CirclePoint> savedPoints = new List<CirclePoint>();
        
        CirclePoint closestPoint = null;
        float closestPointDist = float.PositiveInfinity;
        
        foreach (CirclePoint circlePoint in debugPoints)
        {
            if(!circlePoint.isEndPoint) continue;

            if (circlePoint == current) continue;
            
            if(current.origin == circlePoint.origin) continue;

            float dist = Vector2.Distance(current.point, circlePoint.point);
            if(dist > distanceBetweenPointsOnCircle * 2) continue;

            if(DoesPointsShareNeighbors(current, circlePoint)) continue;

            if (Vector2.Distance(current.origin, circlePoint.origin) > radius * 2)
            {
                savedPoints.Add(circlePoint);
                continue;
            }
            Debug.Log("5");
            if (!(dist < closestPointDist)) continue;
            
            Debug.Log("6");
            closestPoint = circlePoint;
            closestPointDist = dist;
        }

        if (closestPoint != null)
        {
            return closestPoint;
        }
        else
        {
            closestPoint = null;
            closestPointDist = float.PositiveInfinity;
            foreach (CirclePoint savedPoint in savedPoints)
            {
                float dist = Vector2.Distance(current.point, savedPoint.point);
                if (!(dist < closestPointDist)) continue;
                closestPoint = savedPoint;
                closestPointDist = dist;
            }

            return closestPoint;
        }
    }

    private bool DoesPointsShareNeighbors(CirclePoint point1, CirclePoint point2)
    {
        for (int i = 0; i < point1.neighborPoints.Length; i++)
        {
            for (int j = 0; j < point2.neighborPoints.Length; j++)
            {
                if (point2.neighborPoints[j] == null && point1.neighborPoints[i] == null) return false;
                if (point1.neighborPoints[i] == point2.neighborPoints[j]) return true;
            }
        }
        return false;
    }


    // private bool TryToConnectPoints(CirclePoint point1, CirclePoint point2)
    // {
    //     if (point1.connectedPoints >= 2 || point1.connectedPoints >= 2) return false;
    // }

    private void Update()
    {
        distanceBetweenPointsOnCircle = radius * 2 * Mathf.PI / pointsPerCircle * 1.3f;
        debugPoints.Clear();
        for (int i = 0; i < points.Count; i++)
        {
            CreateDebugPoints(points[i], i);
        }
        lines.Clear();
        //CreateLines();
        CreateLine2();
    }

    private void CreateDebugPoints(Vector2 pos, int currentCircle)
    {
        if(pointsPerCircle < Mathf.Epsilon) return;
        bool previusCircleDrawn = false;
        CirclePoint previusCircle = null;
        bool firstCirclePointDrawn = false;
        bool secondCirclePointDrawn = false;
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
                else if (Mathf.Approximately(j, 360 / pointsPerCircle))
                {
                    secondCirclePointDrawn = true;
                }

                if (j + 360 / pointsPerCircle > 359)
                {
                    circlepoint.isEndPoint = !firstCirclePointDrawn;
                    if (secondCirclePointDrawn)
                    {
                        firstCirclePoint.isEndPoint = false;
                    }
                    
                    if (firstCirclePointDrawn)
                    {
                        firstCirclePoint.neighborPoints[0] = circlepoint;
                        circlepoint.neighborPoints[1] = firstCirclePoint;
                    }
                }
                
                if (!previusCircleDrawn)
                {
                    circlepoint.isEndPoint = true;
                }
                else
                {
                    previusCircle.neighborPoints[1] = circlepoint;
                    circlepoint.neighborPoints[0] = previusCircle;
                }
                debugPoints.Add(circlepoint);

                previusCircleDrawn = true;
                previusCircle = circlepoint;
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

                previusCircle = null;
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
