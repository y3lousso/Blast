using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCube : SpawnableObject
{
    public CubeColor cubeColor;

    [SerializeField] AudioClip explodeClip;
    [SerializeField] AudioClip missClip;

    [SerializeField] ParticleSystem explodeParticle;

    public override void DestroyObject()
    {
        AudioSource.PlayClipAtPoint(explodeClip, transform.position);
        ParticleSystem p = Instantiate(explodeParticle,transform.position, transform.rotation);
        GameManager.Instance.results.CorrectHit++;
        Destroy(p.gameObject, 1f);
        Destroy(gameObject);
    }

    public void Miss()
    {
        AudioSource.PlayClipAtPoint(missClip, transform.position);
        Destroy(gameObject);
    }
}



