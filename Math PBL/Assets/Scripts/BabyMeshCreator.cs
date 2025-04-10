using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BabyMeshCreator : MonoBehaviour
{
    Mesh mesh;
    public ButtonPress buttonScript;
    public MeshCollider meshC;
    public Vector3[] vertices;
    public int[] triangles;

    // public int xSize;
    // public int ySize;
    
    private float rotationSpeed;
    private float maxRotationSpeed = 0.75f;

    public int curveSegments = 10; // Controls smoothness of the semicircle
    public float radius = 1f;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshC = GetComponent<MeshCollider>();

        CreateMesh();
        UpdateMesh();
    }

    void FixedUpdate()
    {
        if (ButtonPress.buttonIsPressed)
        {
            rotationSpeed += 0.005f;
            
            // Pizza scale mesh (both evenly and unevenly)
            EvenlyScaleMesh(1.01f); // Slow increase
            UnevenlyScaleMesh(0.99f); // Slow decrease
        }
        else
        {
            rotationSpeed -= 0.005f;
            
            // EvenlyScaleMesh(0.8f); // Fast decrease
            // UnevenlyScaleMesh(1.2f); // Fast increase
        }

        if (rotationSpeed > maxRotationSpeed) { rotationSpeed = maxRotationSpeed; }
        else if (rotationSpeed < 0) { rotationSpeed = 0; }
        
        // Rotate mesh on Z axis
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = RotateX(vertices[i], rotationSpeed);
        }
        
        UpdateMesh();
    }

    /* Vector3 RotateX(Vector3 vertex, float angle)
    {
        float cosTheta = Mathf.Cos(angle);
        float sinTheta = Mathf.Sin(angle);

        float newY = vertex.y * cosTheta - vertex.z * sinTheta;
        float newZ = vertex.y * sinTheta + vertex.z * cosTheta;

        return new Vector3(vertex.x, newY, newZ);
    } */
    
    Vector3 RotateX(Vector3 vertex, float angle)
    {
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg)); // Z axis
        return rotationMatrix.MultiplyPoint3x4(vertex);
        
        /* Quaternion rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.right);
        Vector3 rotated = rotation * vertex;
        return rotated; */
    }
    
    public void EvenlyScaleMesh(float scaleFactor)
    {
        /* Matrix4x4 scaleMatrix = Matrix4x4.Scale(Vector3.one * scaleFactor);

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = scaleMatrix.MultiplyPoint3x4(vertices[i]);
        } */
        
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(
                vertices[i].x * scaleFactor,
                vertices[i].y * scaleFactor,
                vertices[i].z * scaleFactor
            );
        }
    }
    
    public void UnevenlyScaleMesh(float scaleFactor)
    {
        /* Matrix4x4 scaleMatrix = Matrix4x4.Scale(new Vector3(1f, 1f, scaleFactor)); // Z axis

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = scaleMatrix.MultiplyPoint3x4(vertices[i]);
        } */
        
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(
                vertices[i].x * 1,
                vertices[i].y * 1,
                vertices[i].z * scaleFactor
            );
        }
    }

    void CreateMesh()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // front points
        vertices.Add(new Vector3(2, -2, -1));   // 0
        vertices.Add(new Vector3(2, -1, -1));   // 1
        vertices.Add(new Vector3(1, -1, -1));   // 2
        vertices.Add(new Vector3(1, 0, -1));    // 3
        vertices.Add(new Vector3(2, 0, -1));    // 4
        vertices.Add(new Vector3(2, 1, -1));    // 5
        vertices.Add(new Vector3(-2, 1, -1));   // 6
        vertices.Add(new Vector3(-2, 0, -1));   // 7
        vertices.Add(new Vector3(-1, 0, -1));   // 8
        vertices.Add(new Vector3(-1, -1, -1));  // 9
        vertices.Add(new Vector3(-2, -1, -1));  // 10
        vertices.Add(new Vector3(-2, -2, -1));  // 11

        // back points
        vertices.Add(new Vector3(2, -2, 1));    // 12
        vertices.Add(new Vector3(2, -1, 1));    // 13
        vertices.Add(new Vector3(1, -1, 1));    // 14
        vertices.Add(new Vector3(1, 0, 1));     // 15
        vertices.Add(new Vector3(2, 0, 1));     // 16
        vertices.Add(new Vector3(2, 1, 1));     // 17
        vertices.Add(new Vector3(-2, 1, 1));    // 18
        vertices.Add(new Vector3(-2, 0, 1));    // 19
        vertices.Add(new Vector3(-1, 0, 1));    // 20
        vertices.Add(new Vector3(-1, -1, 1));   // 21
        vertices.Add(new Vector3(-2, -1, 1));   // 22
        vertices.Add(new Vector3(-2, -2, 1));   // 23

        // extra points that we forgot to add above and didn't want to place above to avoid having to rearrange the index 
        vertices.Add(new Vector3(1, 1, -1));    // 24
        vertices.Add(new Vector3(-1, 1, -1));   // 25
        vertices.Add(new Vector3(1, 1, 1));     // 26
        vertices.Add(new Vector3(-1, 1, 1));    // 27

        // front curve points
        Vector3 centerFront = new Vector3(0, 1, -1);
        int frontCurveStartIndex = vertices.Count;
        for (int i = 0; i <= curveSegments; i++)
        {
            float angle = Mathf.PI - (Mathf.PI * i / curveSegments);
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius + centerFront.y;
            vertices.Add(new Vector3(x, y, centerFront.z));
        }

        // back curve points
        Vector3 centerBack = new Vector3(0, 1, 1);
        int backCurveStartIndex = vertices.Count;
        for (int i = 0; i <= curveSegments; i++)
        {
            float angle = Mathf.PI - (Mathf.PI * i / curveSegments);
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius + centerBack.y;
            vertices.Add(new Vector3(x, y, centerBack.z));
        }

        // Front faces
        triangles.AddRange(new int[] { 0, 11, 10 });
        triangles.AddRange(new int[] { 10, 1, 0 });
        triangles.AddRange(new int[] { 2, 9, 8 });
        triangles.AddRange(new int[] { 8, 3, 2 });
        triangles.AddRange(new int[] { 4, 7, 6 });
        triangles.AddRange(new int[] { 6, 5, 4 });

        //Front curved face
        int centerFrontIndex = vertices.Count;
        vertices.Add(centerFront);
        for (int i = 0; i < curveSegments; i++)
        {
            triangles.Add(centerFrontIndex);
            triangles.Add(frontCurveStartIndex + i);
            triangles.Add(frontCurveStartIndex + i + 1);
        }

        // Back faces
        triangles.AddRange(new int[] { 22, 23, 12 });
        triangles.AddRange(new int[] { 12, 13, 22 });
        triangles.AddRange(new int[] { 20, 21, 14 });
        triangles.AddRange(new int[] { 14, 15, 20 });
        triangles.AddRange(new int[] { 18, 19, 16 });
        triangles.AddRange(new int[] { 16, 17, 18 });

        // Back curved face
        int centerBackIndex = vertices.Count;
        vertices.Add(centerBack);
        for (int i = 0; i < curveSegments; i++)
        {
            triangles.Add(centerBackIndex);
            triangles.Add(backCurveStartIndex + i + 1);
            triangles.Add(backCurveStartIndex + i);
        }
        
        // Side faces
        triangles.AddRange(new int[] { 12, 23, 11 });
        triangles.AddRange(new int[] { 11, 0, 12 });
        triangles.AddRange(new int[] { 12, 0, 1 });
        triangles.AddRange(new int[] { 1, 13, 12 });
        triangles.AddRange(new int[] { 1, 2, 14 });
        triangles.AddRange(new int[] { 14, 13, 1 });
        triangles.AddRange(new int[] { 14, 2, 3 });
        triangles.AddRange(new int[] { 3, 15, 14 });
        triangles.AddRange(new int[] { 16, 15, 3 });
        triangles.AddRange(new int[] { 3, 4, 16 });
        triangles.AddRange(new int[] { 16, 4, 5 });
        triangles.AddRange(new int[] { 5, 17, 16 });
        triangles.AddRange(new int[] { 11, 23, 22 });
        triangles.AddRange(new int[] { 22, 10, 11 });
        triangles.AddRange(new int[] { 9, 10, 22 });
        triangles.AddRange(new int[] { 22, 21, 9 });
        triangles.AddRange(new int[] { 9, 21, 20 });
        triangles.AddRange(new int[] { 20, 8, 9 });
        triangles.AddRange(new int[] { 20, 19, 7 });
        triangles.AddRange(new int[] { 7, 8, 20 });
        triangles.AddRange(new int[] { 7, 19, 18 });
        triangles.AddRange(new int[] { 18, 6, 7 });
        triangles.AddRange(new int[] { 5, 24, 26 });
        triangles.AddRange(new int[] { 26, 17, 5 });
        triangles.AddRange(new int[] { 25, 6, 18 });
        triangles.AddRange(new int[] { 18, 17, 25 });

        // Side curve face
        for (int i = 0; i < curveSegments; i++)
        {
            int frontA = frontCurveStartIndex + i;
            int frontB = frontCurveStartIndex + i + 1;
            int backA = backCurveStartIndex + i;
            int backB = backCurveStartIndex + i + 1;

            triangles.Add(frontA);
            triangles.Add(backA);
            triangles.Add(backB);

            triangles.Add(frontA);
            triangles.Add(backB);
            triangles.Add(frontB);
        }

        this.vertices = vertices.ToArray();
        this.triangles = triangles.ToArray();
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        //mesh.RecalculateNormals();
        meshC.sharedMesh = mesh;
    }
}
