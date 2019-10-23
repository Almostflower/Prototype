using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Triangle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[3];
        int[] triangles = new int[3];
        for (int i = 0; i < 3; i++)
        {
            float angle = i * 120f * Mathf.Deg2Rad;
            vertices[i] = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
        }
        triangles = new int[3] { 0, 1, 2 };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
