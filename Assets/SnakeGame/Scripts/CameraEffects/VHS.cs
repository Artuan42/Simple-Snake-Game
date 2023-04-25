using UnityEngine;

public class VHS : MonoBehaviour
{
    Material mat;
    public Shader shader;

    public float Curvature = 4;
    public float Width = 1;
    public float CRTSpeed = 0.5f;
    public float CRTSize = 0.4f;
    public float CRTOPacity = 1.8f;

    private void Awake()
    {
        if (mat == null) mat = new Material(shader);


        mat.SetFloat("_Curvature", Curvature);
        mat.SetFloat("_VignetteWidth", Width);
        mat.SetFloat("_CRTSpeed", CRTSpeed);
        mat.SetFloat("_CRTSize", CRTSize);
        mat.SetFloat("_CRTOpacity", CRTOPacity);
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }
}
