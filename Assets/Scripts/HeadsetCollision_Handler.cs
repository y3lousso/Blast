using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsetCollision_Handler : MonoBehaviour {

    VRTK.VRTK_HeadsetCollision headsetCollision;

    private void Awake()
    {
        headsetCollision = GetComponent<VRTK.VRTK_HeadsetCollision>();
    }

    private void OnEnable()
    {
        headsetCollision.HeadsetCollisionDetect += HeadsetCollision_HeadsetCollisionDetect;
        headsetCollision.HeadsetCollisionEnded += HeadsetCollision_HeadsetCollisionEnded;
    }

    private void OnDisable()
    {
        headsetCollision.HeadsetCollisionDetect -= HeadsetCollision_HeadsetCollisionDetect;
        headsetCollision.HeadsetCollisionEnded -= HeadsetCollision_HeadsetCollisionEnded;
    }

    private void HeadsetCollision_HeadsetCollisionDetect(object sender, VRTK.HeadsetCollisionEventArgs e)
    {
        AudioManager.Instance.OnHeadsetCollisionDetected();
    }

    private void HeadsetCollision_HeadsetCollisionEnded(object sender, VRTK.HeadsetCollisionEventArgs e)
    {
        AudioManager.Instance.OnHeadsetCollisionEnded();
    }
}
