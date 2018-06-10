using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControllerManager : MonoBehaviour {

    public static VRControllerManager instance;

    public VRTK.VRTK_ControllerEvents leftControllerEvents;
    public VRTK.VRTK_ControllerEvents rightControllerEvents;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            throw new System.Exception("Singleton error");
        }
    }

    public void PlayHapticBothHand(float strength, float duration, float interval)
    {
        PlayHaptic(VRTK.VRTK_ControllerReference.GetControllerReference(leftControllerEvents.gameObject), strength,duration,interval);
        PlayHaptic(VRTK.VRTK_ControllerReference.GetControllerReference(rightControllerEvents.gameObject), strength, duration, interval);
    }

    public void PlayHaptic(VRTK.VRTK_ControllerReference controllerReference, float strength, float duration, float interval)
    {
        VRTK.VRTK_ControllerHaptics.TriggerHapticPulse(controllerReference, strength, duration, interval);
    }

    public void PlayHaptic(GameObject controller, float strength, float duration, float interval)
    {
        VRTK.VRTK_ControllerHaptics.TriggerHapticPulse(VRTK.VRTK_ControllerReference.GetControllerReference(controller), strength, duration, interval);
    }

}
