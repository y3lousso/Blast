using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCubeOffLimit : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        TargetCube targetCube = other.GetComponent<TargetCube>();
        if(targetCube != null)
        {
            targetCube.DestroyObject();
        }
    }

}
