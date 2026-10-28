using UnityEngine;
using System.Collections;

public class MagicShield : MonoBehaviour
{
    Material material;
    public float speed;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Handle the hit object here
                if (hit.collider.gameObject == this.gameObject)
                {
                    // Perform actions on the hit object
                    StartCoroutine(GetPowerUp());
                }
            }
        }
    }

    IEnumerator GetPowerUp()
    {
        float start = material.GetFloat("_Fresnel_Effect");
        float value = start;
        while(value > 1)
        {
            value -= Time.deltaTime * speed;
            material.SetFloat("_Fresnel_Effect", value);
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        while(value < start)
        {
            value += Time.deltaTime * speed;
            material.SetFloat("_Fresnel_Effect", value);
            yield return null;
        }
    }

}
