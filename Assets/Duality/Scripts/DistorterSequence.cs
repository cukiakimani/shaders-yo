using UnityEngine;
using System.Collections;

public class DistorterSequence : MonoBehaviour
{
    public Vector3[] DistorterStartEndPos;

    public Transform MainSphere;
    private Material _mainSphereMaterial;

    public AnimationCurve RadiusMovement;
    public float RadiusMax = 0.123f;

    [Range(0f, 1f)]
    public float Time = 0f;

    public float AnimTime = 1f;

    private Material _springMaterial;
    private Vector2[] _distortionUVs;

    void Start()
    {
        _springMaterial = GetComponent<SpringShader>().material;
        _mainSphereMaterial = MainSphere.GetComponentInChildren<MeshRenderer>().material;

        // find distortion points by raycasting and save them
        _distortionUVs = new Vector2[2];
        SetDistortionUVs(DistorterStartEndPos[0], 0);
        SetDistortionUVs(DistorterStartEndPos[1], 1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine("AnimateDistorter");
            StartCoroutine("AnimateDistorter");
        }

        SetDistortionUVs(DistorterStartEndPos[0], 0);
        SetDistortionUVs(DistorterStartEndPos[1], 1);

        Time = UtilityFunctions.Map(transform.position.x, DistorterStartEndPos[0].x, DistorterStartEndPos[1].x, 0f, 1f);

        if (Time < 0.5f)
        {
            _springMaterial.SetFloat("_DistortPositionX", _distortionUVs[0].x);
            _springMaterial.SetFloat("_DistortPositionY", _distortionUVs[0].y);
        }
        else
        {
            _springMaterial.SetFloat("_DistortPositionX", _distortionUVs[1].x);
            _springMaterial.SetFloat("_DistortPositionY", _distortionUVs[1].y);
        }

        float r = RadiusMovement.Evaluate(Time) * RadiusMax; // RadiusMax * 0.25f;
        _springMaterial.SetFloat("_Radius", r);
    }

    void SetDistortionUVs(Vector3 pos, int uvIndex)
    {
        Vector3 direction = MainSphere.position - pos;
        direction.Normalize();

        RaycastHit hit;
        if (Physics.Raycast(pos, direction, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(pos, direction * hit.distance, Color.yellow);

            var mat = MainSphere.GetComponentInChildren<MeshRenderer>().material;

            RenderTexture tex = mat.GetTexture("_NoiseTex") as RenderTexture;
            Vector2 pixelUV = hit.textureCoord;
            _distortionUVs[uvIndex] = pixelUV;
            // print(pixelUV);
        }
    }

    IEnumerator AnimateDistorter()
    {
        float t = 0f;

        while (t < AnimTime)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Lerp(DistorterStartEndPos[1].x - 2f, DistorterStartEndPos[0].x + 2f, t / AnimTime);
            transform.position = pos;
            t += UnityEngine.Time.deltaTime;
            yield return null;
        }
    }


    void OnDrawGizmos()
    {
        for (int i = 0; i < DistorterStartEndPos.Length; i++)
        {
            Vector3 posTop = DistorterStartEndPos[i] + Vector3.up * 1f;
            Vector3 posBottom = DistorterStartEndPos[i] + Vector3.down * 1f;

            Debug.DrawLine(posTop, posBottom, Color.red);
        }
    }
}
