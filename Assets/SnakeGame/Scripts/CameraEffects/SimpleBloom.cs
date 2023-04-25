using UnityEngine;

public class SimpleBloom : MonoBehaviour
{
    Material mat;
    public Shader shader;

    public float blurSize;
    public float blurIntensity = 0.35f;

    private void Awake()
    {
        if (mat == null) mat = new Material(shader);

        mat.SetFloat("_Intensity", blurIntensity);
        mat.SetFloat("_BlurSize", blurSize);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }
}
