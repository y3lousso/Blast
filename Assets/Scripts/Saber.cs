using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saber : MonoBehaviour {

    public CubeColor saberColor = CubeColor.Blue;

    public void OnTriggerEnter(Collider other)
    {
        TargetCube targetCube = other.GetComponentInParent<TargetCube>();

        if (targetCube != null && other.CompareTag("TargetCube_CorrectSide") && targetCube.cubeColor == saberColor)
        {            
            Destroy(targetCube.gameObject);
        }
        else if(targetCube != null &&  other.CompareTag("TargetCube_WrongSide"))
        {          
            foreach (Collider col in targetCube.GetComponentsInChildren<Collider>())
            {
                col.enabled = false;
            }
            //TODO : Haptic -> go get script from UnrealInstructor
            //e.triggeredObject.
        }
    }
}

