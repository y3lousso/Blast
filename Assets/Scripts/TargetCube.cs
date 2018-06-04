using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCube : MonoBehaviour {

    private bool isActivated = false;
    private float speed;
    public CubeColor cubeColor;

    public struct TargetCubeEventArgs
    {
        public bool isCorrect;
    }

    public delegate void TargetCubeEventHandler(object sender, TargetCubeEventArgs e);

    public static event TargetCubeEventHandler OnCubeDestroy;


    // Use this for initialization
    void Start () {
        speed = GameManager.Instance.targetCubeSpeed;

        isActivated = true;
    }
	
	// Update is called once per frame
	void Update () {
		if(isActivated == true)
        {
            transform.Translate(- transform.forward * speed * Time.deltaTime);
        }
	}
}



