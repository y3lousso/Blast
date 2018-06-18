using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saber : MonoBehaviour {

    public CubeColor saberColor = CubeColor.Blue;

    private Transform saberEdge;
    private Vector3 saberEdgeLastFramePosition = new Vector3();

    public Vector3 saberEdgeVelocity = new Vector3();

    public void Awake()
    {
        saberEdge = transform.Find("SaberEdge");    
    }

    public void Update()
    {
        saberEdgeVelocity = saberEdge.position - saberEdgeLastFramePosition;
        saberEdgeVelocity.z = 0f;
        saberEdgeLastFramePosition = saberEdge.position;
    }

    public void OnTriggerEnter(Collider other)
    {
        TargetCube targetCube = other.GetComponentInParent<TargetCube>();

        if (targetCube != null)
        {
            bool hasEnoughVelocity = (saberEdgeVelocity.magnitude >= GameManager.Instance.slashIntensityThreshold) ? true : false;
            bool hasCorrectAngle = (Vector3.Dot(-targetCube.transform.up, saberEdgeVelocity.normalized) >= GameManager.Instance.slashAngleThreshold) ? true : false;

            if (hasEnoughVelocity && hasCorrectAngle && saberColor == targetCube.cubeColor)
            {
                targetCube.CorrectHit();
                VRControllerManager.instance.PlayHaptic(transform.parent.gameObject, 7, .1f, .01f);
            }
            else
            {
                targetCube.DestroyObject();
            }
        }
    }
}

