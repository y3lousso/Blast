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
        GameManager.Instance.results.CorrectHit++;

        if(GameManager.Instance.multiplier < 10f)
        {
            GameManager.Instance.multiplier += .5f;
        }

        GameManager.Instance.results.Score += (int)(123f * GameManager.Instance.multiplier);
        Destroy(p.gameObject, 1f);
        Destroy(gameObject);
    }

    public override void DestroyObject()
    {
        AudioSource.PlayClipAtPoint(missClip, transform.position);
        GameManager.Instance.multiplier -= 1f;
        if (GameManager.Instance.multiplier < 1f)
        {
            GameManager.Instance.multiplier = 1f;
        }
        
        Destroy(gameObject);
    }
}



