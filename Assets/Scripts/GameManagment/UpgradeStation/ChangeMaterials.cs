using UnityEngine;

public class ChangeMaterials : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material normalMaterial;
    private bool toggle = false;
    private void ChangeMaterialOutline()
    {
        GetComponent<SpriteRenderer>().material = outlineMaterial;

    }
    private void ChangeMaterialNormal()
    {
        GetComponent<SpriteRenderer>().material = normalMaterial;

    }

    private void ChangeMaterial()
    {
        if (toggle)
        {
            ChangeMaterialNormal();
            toggle = false;
        }
        else
        {
            toggle = true;
            ChangeMaterialOutline();
        }


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeMaterial();
        }

    }

}
