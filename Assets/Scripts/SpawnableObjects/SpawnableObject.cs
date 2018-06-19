using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnableObject : MonoBehaviour {

    private float speed;
    private float lifeTime = 10f;

    void Start()
    {
        speed = GameManager.Instance.targetCubeSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime < 0f)
            {
                DestroyObject();
            }
            else
            {
                transform.Translate(-transform.forward * speed * Time.deltaTime);
            }
        }
    }

    public abstract void DestroyObject();
}
