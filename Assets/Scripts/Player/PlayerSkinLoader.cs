using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinLoader : MonoBehaviour
{
    private string skinName;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private List<Material> skinMaterials;

    // Start is called before the first frame update
    void Awake()
    {
        skinName = "Slime_" + PlayerPrefs.GetString("SlimeSkin"); //Get skin name from PlayerPrefs.

        foreach (Material m in skinMaterials) //Search list of possible skins.
        {
            if (m.name == skinName) //If material name matches skin name.
            {
                meshRenderer.material = m; //Set mesh material to skin material.
            }
        }
    }
}
