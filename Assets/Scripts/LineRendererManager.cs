using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererManager : MonoBehaviour {

    private LineRenderer lineRenderer;

    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;

    private float textureOffset = 0f;

    // Use this for initialization
    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        lineRenderer.SetPosition(0, startPoint.localPosition);
        lineRenderer.SetPosition(1, endPoint.localPosition);

        textureOffset -= Time.deltaTime * 2f;
        if (textureOffset < -10f)
        {
            textureOffset += 10f;
        }
        lineRenderer.sharedMaterials[0].SetTextureOffset("_MainTex", new Vector2(textureOffset, 0f));
    }
}
