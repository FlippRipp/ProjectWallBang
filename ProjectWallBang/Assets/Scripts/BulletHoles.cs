using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoles : MonoBehaviour
{
    
    struct BulletHole
    {
        public Vector3 Position;
        public float Radius;
    }

    [SerializeField] private Camera renderTextureCamera;
    [SerializeField] private GameObject bulletInpactPrefab;
    [SerializeField] private float wallThickness = 0.1f;
    [SerializeField, Tooltip("Number of edges the holes have on the inside.")] private int numberOfEdges = 5;

    [SerializeField] private Material material;
    [SerializeField] private float raidusMod = 1.1f;
    [SerializeField] private bool debugMode = false;
    
    private List<BulletHole> bulletHoles = new List<BulletHole>();

    private List<Vector3> debugPos = new List<Vector3>();


    private Camera mainCamera;
    // Start is called before the first frame update
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    CreateBulletHole(hit.point);
                }
            }
        }
    }

    private void CreateBulletHole(Vector3 pos)
    {
        BulletHole bulletHole = new BulletHole();
        Vector3 hitOffset = transform.InverseTransformPoint(pos);
        hitOffset = new Vector3(-hitOffset.x, -hitOffset.z, hitOffset.y) + new Vector3(0, 0, 1);
        bulletHole.Position = pos - transform.position;

        Transform bullet = Instantiate(bulletInpactPrefab, renderTextureCamera.transform).transform;
        bullet.localPosition = hitOffset;
        bulletHole.Radius = bullet.localScale.x;
        bulletHoles.Add(bulletHole);

        GenerateBarrier();
    }

    private void GenerateBarrier()
    {
        BulletHole bulletHole = bulletHoles[bulletHoles.Count - 1];
        Vector3[] vertices = new Vector3[numberOfEdges * 2];
        int[] triangles = new int[numberOfEdges * 6];
        GameObject hole = new GameObject("Hole");
        hole.transform.parent = transform;
        //hole.transform.rotation = transform.rotation;
        hole.transform.position = transform.position + bulletHole.Position;

        //Instantiate(, transform.position);

        MeshRenderer meshRenderer = hole.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = hole.AddComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        
        //Vector3 bulletHoleEdge = bulletHole.Position + bulletHole.Radius * transform.forward;
        int j = 0;
        for (int i = 0; i < 359; i += 360 / numberOfEdges)
        {
            Vector3 radiusPoint = Quaternion.AngleAxis(i, transform.up) * (bulletHole.Radius * raidusMod * transform.forward);

             vertices[j++] = -radiusPoint;
             vertices[j++] = -radiusPoint - transform.up * 0.1f;
            debugPos.Add(bulletHole.Position - radiusPoint);
            debugPos.Add(bulletHole.Position - radiusPoint - transform.up * 0.1f);
        }

        for (int i = 0; i < numberOfEdges; i++)
        {
            
            int[] tris;
            if (i == numberOfEdges - 1)
            {
                tris = CreateQuad(i * 2, i*2+1, 0, 1);
                Debug.Log("d");
            }
            else
            {
                tris = CreateQuad(i * 2, i*2+1, i*2+2, i*2+3);

            }


            for (int k = 0; k < tris.Length; k++)
            {
                triangles[i * 6 + k] = tris[k];
            }
            
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshRenderer.material = material;
    }

    private int[] CreateQuad(int v1, int v2, int v3, int v4)
    {
        int[] triangles = new int[6];
        
        triangles[5] = triangles[2] = v1; // 5 2
        triangles[4] = v2;//4
        triangles[3] = triangles[1] = v4;//3 1
        triangles[0] = v3; //0
        Debug.Log(v4);

        return triangles;
    }
    
    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            for (int i = 0; i < debugPos.Count; i++)
            {
                Gizmos.DrawSphere(debugPos[i] + transform.position, 0.01f);
            }
        }

    }
}
