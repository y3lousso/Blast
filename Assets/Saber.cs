using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saber : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("saber detection");
        if(other.GetComponent<TargetCube>() != null)
        {
            other.GetComponent<TargetCube>().Destroy();
        }
    }
}
