using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BabyMeshCreator : MonoBehaviour
{
    Mesh mesh;
    public MeshCollider meshC;
    public Vector3[] vertices;
    public int[] triangles;

    public int xSize;
    public int ySize;
    
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshC = GetComponent<MeshCollider>();

        CreateMesh();
        UpdateMesh();
    }

    void Update()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = RotateX(vertices[i], 0.1f);
        }
        
        UpdateMesh();
    }

    Vector3 RotateX(Vector3 vertex, float angle)
    {
        float cosTheta = Mathf.Cos(angle);
        float sinTheta = Mathf.Sin(angle);
        
        float newY = vertex.y * cosTheta - vertex.z * sinTheta;
        float newZ = vertex.y * sinTheta + vertex.z * cosTheta;
        
        return new Vector3(vertex.x, newY, newZ);
    }

    void CreateMesh()
    {
        vertices = new Vector3[]
        {
            new Vector3(0, 0, 0), // 0
            new Vector3(1, 0, 0), // 1
            new Vector3(0, -1, 0), // 2
            new Vector3(1, -0.5f, 0), // 3
            new Vector3(1, 0, 1), // 4
            new Vector3(1, -1, 1), // 5
            new Vector3(0, 0, 1), // 6
            new Vector3(0, -1, 1), // 7
        };

        triangles = new int[]
        {
            0,1,2,
            3,2,1,
            1,4,3,
            4,5,3,
            4,1,0,
            0,6,4,
            0,7,6,
            7,0,2,
            2,3,7,
            7,3,5,
            4,6,7,
            4,7,5
        };
    }
    
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();
        
        meshC.sharedMesh = mesh;
    }
}
