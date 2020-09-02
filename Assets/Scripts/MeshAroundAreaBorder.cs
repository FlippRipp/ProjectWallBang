using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class MeshAroundAreaBorder : MonoBehaviour
{
    public List<Vector2> points = new List<Vector2>();

    [SerializeField] private List<CirclePoint> verticesPoints = new List<CirclePoint>();
    [SerializeField] private float pointsPerCircle = 20;
    [SerializeField] private float meshDepth = 0.1f;
    [SerializeField] private float radiusOffset = 0.1f;
    [SerializeField] private Material wallInsideMaterial;
    [SerializeField] private bool debuging;
    [NonSerialized] public float radius = 20;
    private List<Line> lines = new List<Line>();
    private float distanceBetweenPointsOnCircle;

    private int saftyLock = 0;


    private void Awake()
    {
        distanceBetweenPointsOnCircle = radius * 2 * Mathf.PI / pointsPerCircle * 1.3f;
        //debugPoints.Clear();
        for (int i = 0; i < points.Count; i++)
        {
            CreateDebugPoints(points[i], i);
        }

        //CreateLines();
    }

    private void CreateLines()
    {
        for (int i = 0; i < verticesPoints.Count; i++)
        {
            for (int j = 0; j < verticesPoints.Count; j++)
            {
                if (Vector2.Distance(verticesPoints[i].origin, verticesPoints[j].origin) < radius * 2)
                {
                    if (verticesPoints[i].origin != verticesPoints[j].origin)
                    {
                        if (verticesPoints[i].isEndPoint && verticesPoints[j].isEndPoint)
                        {
                            if (Vector2.Distance(verticesPoints[i].point, verticesPoints[j].point) <
                                distanceBetweenPointsOnCircle * 1.1f)
                            {
                                Line line = new Line(verticesPoints[i].point + (Vector2) transform.position,
                                    verticesPoints[j].point + (Vector2) transform.position);
                                lines.Add(line);
                            }
                        }
                    }
                    else if (Vector2.Distance(verticesPoints[i].point, verticesPoints[j].point) <
                             distanceBetweenPointsOnCircle)
                    {
                        Line line = new Line(verticesPoints[i].point + (Vector2) transform.position,
                            verticesPoints[j].point + (Vector2) transform.position);
                        lines.Add(line);
                    }
                }
            }
        }
    }

    private void CreateLine2()
    {
        ConfigureEndpoint();


        foreach (CirclePoint circlePoint in verticesPoints)
        {
            if (circlePoint.neighbourPoints[0] != null)
            {
                Line line = new Line(circlePoint.point, circlePoint.neighbourPoints[0].point);
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
        foreach (CirclePoint circlePoint in verticesPoints)
        {
            if (!circlePoint.isEndPoint) continue;

            if (circlePoint.neighbourPoints[0] == null)
            {
                CirclePoint closestPoint = FindOtherEndpoint(circlePoint);

                if (closestPoint == null) continue;

                circlePoint.neighbourPoints[0] = closestPoint;
                closestPoint.neighbourPoints[1] = circlePoint;
            }

            if (circlePoint.neighbourPoints[1] == null)
            {
                CirclePoint closestPoint = FindOtherEndpoint(circlePoint);

                if (closestPoint == null) continue;

                circlePoint.neighbourPoints[1] = closestPoint;
                closestPoint.neighbourPoints[0] = circlePoint;
            }
        }
    }

    private CirclePoint FindClosestEndpoint(CirclePoint current, CirclePoint currentNeighbor = null)
    {
        CirclePoint closestPoint = null;
        float closestPointDist = float.PositiveInfinity;

        foreach (CirclePoint circlePoint in verticesPoints)
        {
            if (!circlePoint.isEndPoint || circlePoint == current || circlePoint == currentNeighbor) continue;

            if (circlePoint.origin == current.origin) continue;

            //if (Vector2.Distance(current.origin, circlePoint.origin) > radius * 2) continue;

            if (currentNeighbor != null)
                if (circlePoint.origin == currentNeighbor.origin)
                    continue;

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

        foreach (CirclePoint circlePoint in verticesPoints)
        {
            if (!circlePoint.isEndPoint) continue;

            if (circlePoint == current) continue;

            if (current.origin == circlePoint.origin) continue;

            float dist = Vector2.Distance(current.point, circlePoint.point);
            if (dist > distanceBetweenPointsOnCircle * 2) continue;

            if (DoesPointsShareNeighbours(current, circlePoint)) continue;

            if (IsPointNeighbour(current, circlePoint)) continue;


            if (Vector2.Distance(current.origin, circlePoint.origin) > radius * 2)
            {
                savedPoints.Add(circlePoint);
                continue;
            }

            if (!(dist < closestPointDist)) continue;

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

    private bool IsPointNeighbour(CirclePoint current, CirclePoint point)
    {
        for (int i = 0; i < point.neighbourPoints.Length; i++)
        {
            if (current == null && point.neighbourPoints[i] == null) return false;
            if (current == point.neighbourPoints[i]) return true;
        }

        return false;
    }

    /// <summary>
    /// Does <see cref="point1"/> and <see cref="point2"/> share a neighbor
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns></returns>
    private bool DoesPointsShareNeighbours(CirclePoint point1, CirclePoint point2)
    {
        for (int i = 0; i < point1.neighbourPoints.Length; i++)
        {
            for (int j = 0; j < point2.neighbourPoints.Length; j++)
            {
                if (point2.neighbourPoints[j] == null && point1.neighbourPoints[i] == null) return false;
                if (point1.neighbourPoints[i] == point2.neighbourPoints[j]) return true;
            }
        }

        return false;
    }


    // private bool TryToConnectPoints(CirclePoint point1, CirclePoint point2)
    // {
    //     if (point1.connectedPoints >= 2 || point1.connectedPoints >= 2) return false;
    // }

    public void CreateMeshBarrier()
    {
        saftyLock = 0;
        distanceBetweenPointsOnCircle = radius * radiusOffset * 2 * Mathf.PI / pointsPerCircle * 1.3f;
        lines.Clear();
        verticesPoints.Clear();
        for (int i = 0; i < points.Count; i++)
        {
            CreateDebugPoints(points[i], i);
        }

        CreateLine2();

        GenerateMesh();
    }

    private int[] CreateQuad(int v1, int v2, int v3, int v4)
    {
        int[] triangles = new int[6];

        triangles[5] = triangles[2] = v1; // 5 2
        triangles[4] = v2; //4
        triangles[3] = triangles[1] = v4; //3 1
        triangles[0] = v3; //0

        return triangles;
    }

    private void GenerateMesh()
    {
        Vector3[] vertices = new Vector3[verticesPoints.Count * 2];
        int[] triangles = new int[verticesPoints.Count * 6];
        
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        if (!(meshRenderer = GetComponent<MeshRenderer>()))
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        if (!(meshFilter = GetComponent<MeshFilter>()))
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        Mesh mesh = meshFilter.mesh;

        mesh.Clear();
        for (int i = 0; i < verticesPoints.Count; i++)
        {
            CirclePoint circlepoint = verticesPoints[i];
            Vector3 v1, v2, v3, v4;
            v1 = circlepoint.point;
            v2 = (Vector3) circlepoint.point + new Vector3(0, 0, -meshDepth);
            //v3 = circlepoint.neighbourPoints[0].point;
            //v4 = (Vector3) circlepoint.neighbourPoints[0].point + new Vector3(0, 0, -meshDepth);
            vertices[i * 2] = v1;
            vertices[i * 2 + 1] = v2;

            circlepoint.pointInVertexList = i * 2;
        }

        for (int i = 0; i < verticesPoints.Count; i++)
        {
            
            if(verticesPoints[i] == null) continue;
            if(verticesPoints[i].neighbourPoints[0] == null) continue;

            CirclePoint circlepoint = verticesPoints[i];
            CirclePoint circlePointNeighbour = circlepoint.neighbourPoints[0];
            //int[] tris = CreateQuad(i * 2, i * 2 + 1, i * 2 + 2, i * 2 + 3);
            int[] tris = CreateQuad(circlepoint.pointInVertexList, verticesPoints[i].pointInVertexList + 1,
                circlePointNeighbour.pointInVertexList, circlePointNeighbour.pointInVertexList + 1);

            for (int k = 0; k < tris.Length; k++)
            {
                triangles[i * 6 + k] = tris[k];
            }
        }

        meshRenderer.material = wallInsideMaterial;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }


    private void CreateDebugPoints(Vector2 pos, int currentCircle)
    {
        if (pointsPerCircle < Mathf.Epsilon) return;
        bool previusCircleDrawn = false;
        CirclePoint previusCircle = null;
        bool firstCirclePointDrawn = false;
        bool secondCirclePointDrawn = false;
        CirclePoint firstCirclePoint = new CirclePoint();
        for (float j = 0; j < 359; j += (float) 360 / pointsPerCircle)
        {
            Vector2 radiusPoint = Quaternion.AngleAxis(j, Vector3.forward) * (radius * radiusOffset * Vector3.up);
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
                        firstCirclePoint.neighbourPoints[0] = circlepoint;
                        circlepoint.neighbourPoints[1] = firstCirclePoint;
                    }
                }

                if (!previusCircleDrawn)
                {
                    circlepoint.isEndPoint = true;
                }
                else
                {
                    previusCircle.neighbourPoints[1] = circlepoint;
                    circlepoint.neighbourPoints[0] = previusCircle;
                }

                verticesPoints.Add(circlepoint);

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
                    if (verticesPoints.Count > 0)
                        verticesPoints[verticesPoints.Count - 1].isEndPoint = true;
                }

                previusCircle = null;
                previusCircleDrawn = false;
            }
        }
    }

    public bool CheckIntersectionWithPoints(Vector2 pos, int currentCircle)
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (currentCircle == i) continue;
            if (Vector2.Distance(pos, points[i]) < radius * radiusOffset)
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (!debuging)
            return;

        for (int i = 0; i < verticesPoints.Count; i++)
        {
            if (verticesPoints[i].isEndPoint)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.grey;
            }

            Gizmos.DrawSphere(transform.position + transform.TransformDirection((Vector3) verticesPoints[i].point),
                0.01f);
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < points.Count; i++)
        {
            Gizmos.DrawSphere(transform.position + transform.TransformDirection((Vector3) points[i]), 0.02f);
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < lines.Count; i++)
        {
            Gizmos.DrawLine(transform.position + transform.TransformDirection((Vector3) lines[i].point1),
                transform.position + transform.TransformDirection((Vector3) lines[i].point2));
        }
    }
}