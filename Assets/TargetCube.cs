using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCube : MonoBehaviour {

    public bool isActivated = false;
    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isActivated == true)
        {
            transform.Translate(- transform.forward * speed * Time.deltaTime);
        }
	}

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
