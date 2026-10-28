using UnityEngine;
using UnityEngine.UI;

public class LastShader : MonoBehaviour
{
    public Renderer WaterRenderer;
    Material Water_Material;
    public Slider WaterSlider;
    public Color[] WaterColor;
    public Renderer[] StoreRenderer;
    public Material SnowMaterial;
    public Vector2 StarTiling;

    private void Start()
    {
        Water_Material = WaterRenderer.material;
    }

    public void GetSnowMaterial()
    {
        for (int i = 0; i < StoreRenderer.Length; i++)
        {
            StoreRenderer[i].materials[0] = SnowMaterial;
        }
    }

    private void Update()
    {
        Water_Material.SetFloat("_WaterDepth", WaterSlider.value);
        Water_Material.SetColor("_WaterColor", WaterColor[0]);
        Water_Material.SetColor("_DepthColor", WaterColor[1]);
        Water_Material.SetColor("_FoamColor", WaterColor[2]);

        RenderSettings.skybox.SetVector("_Star_Tiling", StarTiling);
    }
}
