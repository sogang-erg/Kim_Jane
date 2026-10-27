using UnityEngine;

public class PointLightScript : MonoBehaviour
{
    public Light pointLight;
    Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (pointLight == null || material == null) return;
        Vector3 lightPosition = pointLight.transform.position;
        material.SetVector("_lightposition", lightPosition);
    }
}
