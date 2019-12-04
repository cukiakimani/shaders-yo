using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode]
public class MetaballQuad : MonoBehaviour
{
    [SerializeField, Range(1, 4000)] private int numBlobs = 5;

    [SerializeField] private float borderWidth = 0.2f;

    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;

    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    [Space]

    [SerializeField] private Transform blob1;
    [SerializeField] private Transform blob2;

    private Material metaballMaterial;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    private Vector4[] blobs;
    private Transform[] blobTransforms;
    private Vector3[] blobVelocities;

    private Bounds bounds;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        metaballMaterial = meshRenderer.sharedMaterial;

        float minX = transform.position.x + borderWidth;
        float maxX = transform.position.x + (1- borderWidth);

        float minY = transform.position.y + borderWidth;
        float maxY = transform.position.y + (1- borderWidth);

        bounds = new Bounds();
        bounds.min = new Vector3(minX, minY);
        bounds.max = new Vector3(maxX, maxY);

        CreateQuadToCameraSize();
        CreateBlobs();
    }

    void Update()
    {
        UtilityFunctions.DebugDrawRect(bounds.center, bounds.size.x, bounds.size.y, Color.green);

        UpdateBlobs();
        DebugDrawBlobs();
    }

    private void DebugDrawBlobs()
    {
        UtilityFunctions.DebugDrawCircle(blob1.position, blob1.localScale.x, Color.white);
        UtilityFunctions.DebugDrawCircle(blob2.position, blob2.localScale.x, Color.white);

        for (int i = 0; i < numBlobs; i++)
        {
            UtilityFunctions.DebugDrawCircle(blobTransforms[i].position, blobTransforms[i].localScale.x, Color.white);
        }
    }

    private void UpdateBlobs()
    {
        metaballMaterial.SetVector("_C0", transform.InverseTransformPoint(blob1.position));
        metaballMaterial.SetVector("_C1", transform.InverseTransformPoint(blob2.position));
        
        metaballMaterial.SetFloat("_R0", blob1.localScale.x);
        metaballMaterial.SetFloat("_R1", blob2.localScale.x);

        for (int i = 0; i < numBlobs; i++)
        {
            MoveBlob(blobTransforms[i], ref blobVelocities[i]);

            Vector3 invPosition = transform.InverseTransformPoint(blobTransforms[i].position);
            
            blobs[i].x = invPosition.x;
            blobs[i].y = invPosition.y;
            blobs[i].z = blobTransforms[i].localScale.x;
        }

        metaballMaterial.SetInt("_Blobs_Length", numBlobs);
        metaballMaterial.SetVectorArray("_Blobs", blobs);
    }

    private void CreateBlobs()
    {
        blobs = new Vector4[numBlobs];
        blobTransforms = new Transform[numBlobs];
        blobVelocities = new Vector3[numBlobs];

        for (int i = 0; i < numBlobs; i++)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            float r = Random.Range(minRadius, maxRadius);
            float s = Random.Range(minSpeed, maxSpeed);

            Vector3 vel = Random.onUnitSphere * s;
            vel.z = 0f;
            blobVelocities[i] = vel;

            blobs[i] = new Vector4(x, y, r);

            var blobObject = new GameObject("blob");
            Vector3 scale = Vector3.one * r;
            Vector3 position = new Vector3(x, y);

            var blobTransform = blobObject.transform;
            blobTransform.position = position;
            blobTransform.localScale = scale;
            blobTransform.SetParent(transform);

            blobTransforms[i] = blobTransform;
        }
    }

    private void MoveBlob(Transform t, ref Vector3 velocity)
    {
        // check boundaries and bounce off
        if (t.position.x <= bounds.min.x || t.position.x >= bounds.max.x)
        {
            velocity.x *= -1f;
        }

        if (t.position.y <= bounds.min.y || t.position.y >= bounds.max.y)
        {
            velocity.y *= -1f;
        }

        // move postion with speed
        t.position += velocity * Time.deltaTime;

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
}
