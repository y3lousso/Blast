using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour {

    private AudioSource audioSource;

    [Header("Settings")]
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private TargetCube targetCubePrefab;

    [Range(1,10)]
    public float difficulty = 5f;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
        StartCoroutine("CubeSpawnerTest");
    }
	
    private IEnumerator CubeSpawnerTest()
    {     
        while (true)
        {
            SpawnTargetCube();
            yield return new WaitForSeconds(5f / difficulty);
        }
    }

    private void SpawnTargetCube()
    {
        Vector3 randomizer = new Vector3(Random.Range(-1, 1), Random.Range(0, .5f), 0);
        TargetCube current = Instantiate(targetCubePrefab, transform.position + randomizer, transform.rotation);
        current.speed = difficulty;
        current.isActivated = true;

    }
}
