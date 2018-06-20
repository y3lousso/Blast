using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCube : SpawnableObject
{
    public CubeColor cubeColor;

    [SerializeField] AudioClip explodeClip;
    [SerializeField] AudioClip missClip;

    [SerializeField] ParticleSystem explodeParticle;

    public void CorrectHit()
    {
        AudioSource.PlayClipAtPoint(explodeClip, transform.position);
        ParticleSystem p = Instantiate(explodeParticle,transform.position, transform.rotation);
        ScoreManager.Instance.CorrectHit();
        
        Destroy(p.gameObject, 1f);
        Destroy(gameObject);
    }

    public override void DestroyObject()
    {
        AudioSource.PlayClipAtPoint(missClip, transform.position);
        ScoreManager.Instance.MissHit();                
        Destroy(gameObject);
    }
}



