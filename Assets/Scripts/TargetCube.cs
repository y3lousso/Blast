using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCube : MonoBehaviour {

    private float speed;
    public CubeColor cubeColor;

    [SerializeField] AudioClip explodeClip;
    [SerializeField] AudioClip missClip;

    [SerializeField] ParticleSystem explodeParticle;

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
        ParticleSystem p = Instantiate(explodeParticle,transform.position, transform.rotation);
        Destroy(p.gameObject, 1f);
        Destroy(gameObject);
    }

    public void Miss()
    {
        AudioSource.PlayClipAtPoint(missClip, transform.position);
        Destroy(gameObject);
    }
}



