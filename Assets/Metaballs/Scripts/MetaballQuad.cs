using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaballQuad : MonoBehaviour
{
    [SerializeField] private Transform blob1;
    [SerializeField] private Transform blob2;

    private Material metaballMaterial;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        metaballMaterial = meshRenderer.material;

        CreateQuadToCameraSize();
    }

    void Update()
    {
        metaballMaterial.SetVector("_C0", transform.InverseTransformPoint(blob1.position));
        metaballMaterial.SetVector("_C1", transform.InverseTransformPoint(blob2.position));
    }

    private void CreateQuadToCameraSize()
    {
        float orthagraphicSize = Camera.main.orthographicSize;
        float aspectRatio = Camera.main.aspect;

        float height = orthagraphicSize * 2f;
        float width = height * aspectRatio;

        Mesh mesh = new Mesh();
        
        Vector3[] verts = new Vector3[]{
                new Vector3(0f, 0f),
                new Vector3(0f, 1f),
                new Vector3(1f, 1f),
                new Vector3(1f, 0f)
            };
        
        int[] tris = new int[]{
            0, 1, 2,
            0, 2, 3
        };

        Vector2[] uvs = new Vector2[]{
            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f)
        };

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;

        meshFilter.mesh = mesh;
    }

    private void DebugDrawRect(Vector3 position, float width, float height, Color color)
    {
        var topLeft = position + new Vector3(-width * 0.5f, height * 0.5f);
        var topRight = position + new Vector3(width * 0.5f, height * 0.5f);
        var bottomLeft = position + new Vector3(-width * 0.5f, -height * 0.5f);
        var bottomRight = position + new Vector3(width * 0.5f, -height * 0.5f);

        Debug.DrawLine(bottomLeft, topLeft, color);
        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topRight, bottomRight, color);
        Debug.DrawLine(bottomRight, bottomLeft, color);
    }

    private void DebugDrawCircle(Vector3 position, float radius, Color color)
    {
        var inc = 36f;
        var angle = 360f / inc;
        var a = position + Vector3.up * radius;

        for (int i = 0; i < inc; i++)
        {
            var b = a - position;
            b = UtilityFunctions.RotateVector(angle, b);
            b += position;
            Debug.DrawLine(a, b, color);
            a = b;
        }
    }

    private void DebugDrawCross(Vector3 position, float size, Color color, float duration = 0f)
    {
        Vector3 top = position + Vector3.up * size * 0.5f;
        Vector3 bottom = position + Vector3.down * size * 0.5f;
        Vector3 right = position + Vector3.right * size * 0.5f;
        Vector3 left = position + Vector3.left * size * 0.5f;

        Debug.DrawLine(top, bottom, color, duration);
        Debug.DrawLine(right, left, color, duration);
    }
}
