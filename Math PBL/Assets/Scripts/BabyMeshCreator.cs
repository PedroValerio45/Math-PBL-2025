using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BabyMeshCreator : MonoBehaviour
{
    Mesh mesh;
    public ButtonPress buttonScript;
    public MeshCollider meshC;
    public int[] triangles;
    public Vector3[] vertices;

    private float rotationSpeed;
    private float maxRotationSpeed = 1.3f;

    private float scale;
    private float maxScale = 0.125f;

    public int babyCurveSegments = 10;
    public float radius = 1f;

    public enum Axis { x, y, z }

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
            rotationSpeed += 0.003f;

            if (rotationSpeed > maxRotationSpeed)
                scale += 0.0005f;
        }
        else
        {
            rotationSpeed -= 0.003f;
            scale -= 0.0005f;
        }

        if (rotationSpeed > maxRotationSpeed)
        {
            rotationSpeed = maxRotationSpeed;
        }
        else if (rotationSpeed < 0)
        {
            rotationSpeed = 0;
        }

        if (scale > maxScale)
        {
            scale = maxScale;
        }
        else if (scale < 0)
        {
            scale = 0;
        }

        if (rotationSpeed > 0)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                if (ButtonPress.buttonIsPressed)
                    vertices[i] = Matrix_Rotate(vertices[i], rotationSpeed, Axis.z);
                else
                    vertices[i] = Quaternion_Rotate(vertices[i], rotationSpeed, Axis.z);
            }
        }

        if (scale > 0)
        {
            if (ButtonPress.buttonIsPressed)
            {
                if (scale < maxScale && rotationSpeed >= maxRotationSpeed)
                {
                    EvenlyScaleMesh(1.005f);
                    UnevenlyScaleMesh(1, 1, 0.9915f);
                }
            }
            else
            {
                if (scale > 0)
                {
                    EvenlyScaleMesh(0.995f);
                    UnevenlyScaleMesh(1, 1, 1.0085f);
                }
            }
        }

        UpdateMesh();
    }



    //          ROTATION WITH MATRIX
    Vector3 Matrix_Rotate(Vector3 vertex, float angle, Axis axis)
    {
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);

        float[,] rotationMatrix;

        switch (axis)
        {
            default:
                rotationMatrix = new float[3, 3];
            break;

            case Axis.x:
                rotationMatrix = new float[3, 3]
                {
                    { 1,   0,    0  },
                    { 0,  cos, -sin },
                    { 0,  sin,  cos }
                };
            break;

            case Axis.y:
                rotationMatrix = new float[3, 3]
                {
                    { cos,  0,  sin },
                    {  0 ,  1,   0  },
                    {-sin,  0,  cos }
                };
            break;

            case Axis.z:
                rotationMatrix = new float[3, 3]
                {
                    { cos, -sin,  0 },
                    { sin,  cos,  0 },
                    {  0 ,   0 ,  1 }
                };
            break;
        }

        float x = rotationMatrix[0, 0] * vertex.x + rotationMatrix[0, 1] * vertex.y + rotationMatrix[0, 2] * vertex.z;
        float y = rotationMatrix[1, 0] * vertex.x + rotationMatrix[1, 1] * vertex.y + rotationMatrix[1, 2] * vertex.z;
        float z = rotationMatrix[2, 0] * vertex.x + rotationMatrix[2, 1] * vertex.y + rotationMatrix[2, 2] * vertex.z;

        return new Vector3(x, y, z);
    }


    //          ROTATION WITH QUATERNIONS
    Vector3 Quaternion_Rotate(Vector3 vertex, float angle, Axis axis)
    {
        float halfAngle = angle / 2f;
        float sinHalf = Mathf.Sin(halfAngle);
        float cosHalf = Mathf.Cos(halfAngle);

        float w = cosHalf;
        float x = 0f;
        float y = 0f;
        float z = 0f;

        switch (axis)
        {
            case Axis.x:
                x = sinHalf;
            break;

            case Axis.y:
                y = sinHalf;
            break;

            case Axis.z:
                z = sinHalf;
            break;
        }

        float vx = vertex.x;
        float vy = vertex.y;
        float vz = vertex.z;

        float qw = -x * vx - y * vy - z * vz;
        float qx = w * vx + y * vz - z * vy;
        float qy = w * vy + z * vx - x * vz;
        float qz = w * vz + x * vy - y * vx;

        float rx = qx * w - qw * x - qy * z + qz * y;
        float ry = qy * w - qw * y - qz * x + qx * z;
        float rz = qz * w - qw * z - qx * y + qy * x;

        return new Vector3(rx, ry, rz);
    }



    //           EVEN SCALING FUNCTION
    public void EvenlyScaleMesh(float scaleFactor)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(
                vertices[i].x * scaleFactor,
                vertices[i].y * scaleFactor,
                vertices[i].z * scaleFactor
            );
        }
    }

    //          UNEVEN SCALING FUNCTIONS
    public void UnevenlyScaleMesh(float scaleFactor_x, float scaleFactor_y, float scaleFactor_z) // we didn't use Vector3 because we wanted to make the function more intuitive to use
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(
                vertices[i].x * scaleFactor_x,
                vertices[i].y * scaleFactor_y,
                vertices[i].z * scaleFactor_z
            );
        }
    }



    //            MANUALLY-BUILT BABY MESH
    void CreateMesh()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // front points
        vertices.Add(new Vector3( 2, -2, -1));   // 0
        vertices.Add(new Vector3( 2, -1, -1));   // 1
        vertices.Add(new Vector3( 1, -1, -1));   // 2
        vertices.Add(new Vector3( 1,  0, -1));   // 3
        vertices.Add(new Vector3( 2,  0, -1));   // 4
        vertices.Add(new Vector3( 2,  1, -1));   // 5
        vertices.Add(new Vector3(-2,  1, -1));   // 6
        vertices.Add(new Vector3(-2,  0, -1));   // 7
        vertices.Add(new Vector3(-1,  0, -1));   // 8
        vertices.Add(new Vector3(-1, -1, -1));   // 9
        vertices.Add(new Vector3(-2, -1, -1));   // 10
        vertices.Add(new Vector3(-2, -2, -1));   // 11

        // back points
        vertices.Add(new Vector3( 2, -2,  1));   // 12
        vertices.Add(new Vector3( 2, -1,  1));   // 13
        vertices.Add(new Vector3( 1, -1,  1));   // 14
        vertices.Add(new Vector3( 1,  0,  1));   // 15
        vertices.Add(new Vector3( 2,  0,  1));   // 16
        vertices.Add(new Vector3( 2,  1,  1));   // 17
        vertices.Add(new Vector3(-2,  1,  1));   // 18
        vertices.Add(new Vector3(-2,  0,  1));   // 19
        vertices.Add(new Vector3(-1,  0,  1));   // 20
        vertices.Add(new Vector3(-1, -1,  1));   // 21
        vertices.Add(new Vector3(-2, -1,  1));   // 22
        vertices.Add(new Vector3(-2, -2,  1));   // 23

        // extra points that we forgot to add above and instead put below to avoid having to rearrange the whole index 
        vertices.Add(new Vector3( 1,  1, -1));   // 24
        vertices.Add(new Vector3(-1,  1, -1));   // 25
        vertices.Add(new Vector3( 1,  1,  1));   // 26
        vertices.Add(new Vector3(-1,  1,  1));   // 27

        // front curve points
        Vector3 centerFront = new Vector3(0, 1, -1);
        int frontCurveStartIndex = vertices.Count;
        for (int i = 0; i <= babyCurveSegments; i++)
        {
            float angle = Mathf.PI - (Mathf.PI * i / babyCurveSegments);
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius + centerFront.y;
            vertices.Add(new Vector3(x, y, centerFront.z));
        }

        // back curve points
        Vector3 centerBack = new Vector3(0, 1, 1);
        int backCurveStartIndex = vertices.Count;
        for (int i = 0; i <= babyCurveSegments; i++)
        {
            float angle = Mathf.PI - (Mathf.PI * i / babyCurveSegments);
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

        // Front curved face
        int centerFrontIndex = vertices.Count;
        vertices.Add(centerFront);
        for (int i = 0; i < babyCurveSegments; i++)
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
        for (int i = 0; i < babyCurveSegments; i++)
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
        for (int i = 0; i < babyCurveSegments; i++)
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
