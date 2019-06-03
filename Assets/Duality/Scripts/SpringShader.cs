using UnityEngine;
using System.Collections;

public class SpringShader : MonoBehaviour
{

    public Material material; // material with spring shader GPGPU 
    public RenderTexture texture; // render texture to put on actual geometry
    private RenderTexture buffer; // buffer for in between

    public Texture initialTexture; // first texture

    void Start()
    {
        // add the initial texture to the render texture 
        Graphics.Blit(initialTexture, texture);
        buffer = new RenderTexture(texture.width, texture.height, texture.depth, texture.format);
    }

    // Postprocess the image
    public void UpdateTexture()
    {
        material.SetFloat("_DeltaTime", Time.fixedDeltaTime);
        Graphics.Blit(texture, buffer, material);
        Graphics.Blit(buffer, texture);
    }

    void FixedUpdate()
    {
        UpdateTexture();
    }


}
