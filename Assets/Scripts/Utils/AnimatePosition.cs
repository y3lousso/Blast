using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>  
/// 	This class is used to animate GameObjects
///		The objects can be animated as loops or as single time animations
/// </summary>
/// <remarks>
/// 	Used by traps
/// </remarks>
public class AnimatePosition : MonoBehaviour {

    [Header("Animation settings")]
    public bool runOnStart = false;
    public bool running = false;
    public float refreshTime = 0.01f;
    public float duration = 3f;

    [Header("Movement settings")]
    public AnimationCurve posXCurve = AnimationCurve.Constant(0, 1, 0);
	public AnimationCurve posYCurve = AnimationCurve.Constant(0, 1, 0);
	public AnimationCurve posZCurve = AnimationCurve.Constant(0, 1, 0);

	public AnimationCurve rotXCurve = AnimationCurve.Constant(0, 1, 0);
	public AnimationCurve rotYCurve = AnimationCurve.Constant(0, 1, 0);
	public AnimationCurve rotZCurve = AnimationCurve.Constant(0, 1, 0);


	/// <summary>  
	///		Starts the animation if runOnStart is true
	/// </summary> 
	void Start()
    {
		if (runOnStart)
        {
			StartAnimation();
		}
	}

	/// <summary>  
	/// 	Run the animation if not already running
	/// </summary> 
	public void StartAnimation ()
    {
		if (!running)
        {
            StartCoroutine("Play");
        }			
	}

    /// <summary>  
	/// 	Run the animation in reverse if not already running
	/// </summary> 
	public void ReverseAnimation()
    {
        if (!running)
        {
            StartCoroutine("Reverse");
        }            
    }

	/// <summary>  
	/// 	Coroutine running the animation
	/// </summary> 
	IEnumerator Play ()
    {
		Vector3 startPos = transform.localPosition;
		Vector3 startRot = transform.localRotation.eulerAngles;

		float currentTime = 0;
		running = true;

        Debug.Log("play");
        while (currentTime< duration)
        {
            currentTime += Time.deltaTime;

            transform.localPosition = startPos + new Vector3(posXCurve.Evaluate(currentTime), posYCurve.Evaluate(currentTime), posZCurve.Evaluate(currentTime));
			transform.localRotation = Quaternion.Euler(startRot + new Vector3(rotXCurve.Evaluate(currentTime), rotYCurve.Evaluate(currentTime), rotZCurve.Evaluate(currentTime)));

			yield return new WaitForSeconds(refreshTime);
		}

		running = false;
	}

    /// <summary>  
	/// 	Coroutine running the animation
	/// </summary> 
	IEnumerator Reverse()
    {
        Vector3 startPos = transform.localPosition;
        Vector3 startRot = transform.localRotation.eulerAngles;

        float currentTime = 0;
        running = true;

        Debug.Log("reverse");
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            transform.localPosition = startPos - new Vector3(posXCurve.Evaluate(currentTime), posYCurve.Evaluate(currentTime), posZCurve.Evaluate(currentTime));
            transform.localRotation = Quaternion.Euler(startRot - new Vector3(rotXCurve.Evaluate(currentTime), rotYCurve.Evaluate(currentTime), rotZCurve.Evaluate(currentTime)));

            yield return new WaitForSeconds(refreshTime);
        }

        running = false;
    }
}
