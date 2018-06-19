using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereManager : MonoBehaviour {

    public static AtmosphereManager Instance;
    public List<MaterialSwapper> materialSwappers;

    public Material redMaterial;
    public Material blueMaterial;

    private bool isBlue = true;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception("Can't have multiple atmosphere manager.");
        }
    }

    // Use this for initialization
    void Start () {
        materialSwappers = new List<MaterialSwapper>();
        materialSwappers.AddRange(FindObjectsOfType<MaterialSwapper>());
    }

    public void StartToggleColor(float bpm, float offset) {
        float timeBetweenChange = 16*60/bpm; // equals to 16 notes
        InvokeRepeating("ToggleColor", offset, timeBetweenChange);
    }

    public void ToggleColor() {
            if(!isBlue)
            {
                SetAmbiance(CubeColor.Blue);
            }
            else
            {
                SetAmbiance(CubeColor.Red);
            }
            isBlue = !isBlue;
    }
	
	public void SetAmbiance(CubeColor color)
    {
        if(color == CubeColor.Blue)
        {
            foreach(MaterialSwapper m in materialSwappers)
            {
                m.SetMaterial(blueMaterial);
            }              
        }
        else
        {
            foreach (MaterialSwapper m in materialSwappers)
            {
                m.SetMaterial(redMaterial);
            }
        }
    }


}
