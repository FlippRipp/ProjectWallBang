using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutHole : MonoBehaviour
{

    private Camera mainCamera;

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    
    // Start is called before the first frame update
    private void Awake()
    {
        mainCamera = Camera.main;
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                 int triangle = GetNeighbour(hit.triangleIndex);
                 DeleteQuad(triangle, hit.triangleIndex);
            }
        }
    }

    private int FindTriangle(Vector3 v1, Vector3 v2, int triIndex)
    {
        Mesh mesh = meshFilter.mesh;
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        int i = 0;
        int j = 0;
        int found = 0;
        while (j < triangles.Length)
        {
            if (j / 3 != triIndex)
            {
                if (vertices[triangles[j]] == v1 &&
                    (vertices[triangles[j + 1]] == v2 || vertices[triangles[j + 2]] == v2))
                {
                    return j/3;
                }
                else if (vertices[triangles[j]] == v2 &&
                    (vertices[triangles[j + 1]] == v1 || vertices[triangles[j + 2]] == v1))
                {
                    return j/3;

                }
                else if (vertices[triangles[j+1]] == v2 &&
                    (vertices[triangles[j]] == v1 || vertices[triangles[j + 2]] == v1))
                {
                    return j/3;

                }
                else if (vertices[triangles[j+1]] == v1 &&
                    (vertices[triangles[j]] == v2 || vertices[triangles[j + 2]] == v2))
                {
                    return j/3;
                }
            }

            j += 3;
        }

        return -1;
    }

    private int GetNeighbour(int triangleIndex)
    {
        Mesh mesh = meshFilter.mesh;
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        
        Vector3 p0 = vertices[triangles[triangleIndex * 3]];
        Vector3 p1 = vertices[triangles[triangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[triangleIndex * 3 + 2]];

        float edge1 = Vector3.Distance(p0, p1);
        float edge2 = Vector3.Distance(p0, p2);
        float edge3 = Vector3.Distance(p1, p2);

        Vector3 shared1;
        Vector3 shared2;
        if (edge1 > edge2 && edge1 > edge3)
        {
            shared1 = p0;
            shared2 = p1;
        }
        else if (edge2 > edge1 && edge2 > edge3)
        {
            shared1 = p0;
            shared2 = p2;
        }
        else
        {
            shared1 = p1;
            shared2 = p2;
        }

        int v1 = FindVertex(shared1);
        int v2 = FindVertex(shared2);
        
        
        
        return FindTriangle(vertices[v1], vertices[v2], triangleIndex);
    }

    private int FindVertex(Vector3 vertex)
    {
        Vector3[] vertices = meshFilter.mesh.vertices;
        for (int i = 0; i < vertices.Length ; i++)
        {
            if (vertices[i] == vertex)
            {
                return i;
            }
        }

        return -1;
    }

    private void DeleteQuad(int triangleIndex1, int triangleIndex2)
    {
        Mesh mesh = meshFilter.mesh;
        int[] oldTriangles = mesh.triangles;
        int[] newTriangles = new int[mesh.triangles.Length - 6];

        int j = default;
        int i = default;
        while (j < mesh.triangles.Length)
        {
            if (j != triangleIndex1 * 3 && j != triangleIndex2*3)
            {
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
            }
            else
            {
                j += 3;
            }
        }

        meshFilter.mesh.triangles = newTriangles;
        meshCollider.sharedMesh = mesh;

    }

    private void DeleteTri(int triangleIndex)
    {
        Mesh mesh = meshFilter.mesh;
        int[] oldTriangles = mesh.triangles;
        int[] newTriangles = new int[mesh.triangles.Length - 3];

        int j = default;
        int i = default;
        while (j < mesh.triangles.Length)
        {
            if (j != triangleIndex * 3)
            {
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
            }
            else
            {
                j += 3;
            }
        }

        meshFilter.mesh.triangles = newTriangles;
        meshCollider.sharedMesh = mesh;
    }
}
