using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(MeshCollider),typeof(MeshFilter),typeof(MeshRenderer))]
[ExecuteInEditMode]
public class MUATerrainGenerate : MonoBehaviour
{


    private Texture2D heightMap;
    [SerializeField, Range(0.2f, 20)] 
    private float altitude = 10.0f;

    private Vector3[] vertices;
    private int[] triangles;

    Mesh mesh;

    float ColorRMS(Color color)
    {
        float r = color.r;
        float g = color.g;
        float b = color.b;
        float rms = Mathf.Sqrt(r * r + g * g + b * b);
        return (altitude * (rms));
    }

    void Awake()
    {

        heightMap = Resources.Load<Texture2D>("example");
        int height = heightMap.height;
        int width = heightMap.width;

        vertices = new Vector3[height * width];
        triangles = new int[6 * (height - 1) * (width - 1)];
        for (int i=0;i<height;i++)
        {
            for(int j=0;j<width;j++)
            {
                float value = ColorRMS(heightMap.GetPixel(i, j));
                vertices[i * width + j] = new Vector3(i, value, j);
                Debug.Log(i + " " + j + " ---> " + value);
            }
        }
        int triangleCount = 2 * (height - 1) * (width - 1);
        for(int i=0;i<triangleCount;i+=6)
        {
            int start = i / 6;
            triangles[i + 0] = start;
            triangles[i + 1] = start + 1;
            triangles[i + 2] = start + width;

            triangles[i + 3] = start + 1;
            triangles[i + 4] = start + width + 1;
            triangles[i + 5] = start + width;
        }
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }


}
