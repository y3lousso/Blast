using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCube : MonoBehaviour {

    private float speed;
    public CubeColor cubeColor;

    public AudioClip explodeClip;
    public AudioClip missClip;

    public struct TargetCubeEventArgs
    {
        public bool isCorrect;
    }

    public delegate void TargetCubeEventHandler(object sender, TargetCubeEventArgs e);

    public static event TargetCubeEventHandler OnCubeDestroy;

    // Use this for initialization
    void Start ()
    {
        speed = GameManager.Instance.targetCubeSpeed;
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(- transform.forward * speed * Time.deltaTime);
	}

    public void Explode()
    {
        AudioSource.PlayClipAtPoint(explodeClip, transform.position);       
        Destroy(gameObject);
    }

    public void Miss()
    {
        AudioSource.PlayClipAtPoint(missClip, transform.position);
        Destroy(gameObject);
    }
}



