using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; set; }

    private AudioSource audioSource;
    public AudioData audioData;
    public TargetCubeSpawner targetCubeSpawner;

    [Range(1, 10)]
    public float targetCubeSpeed = 5f;

    public float timeBeforeNextBeat = 0f;
    public float currentTimeIndicator = 0;
    public int currentIndex = 0;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception("Can't have multiple game manager.");
        }
    }

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine("StartMusic");
        InvokeRepeating("StartSpawningCubes2", audioData.startingOffset, 60f / audioData.beatPerMinute);
    }

    private IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(audioData.startingOffset);
        audioSource.clip = audioData.audioClip;
        audioSource.Play();
    }

    private void StartSpawningCubes2()
    {
        targetCubeSpawner.SpawnTargetCubes(audioData.targetCubesData.FindAll(t => t.Id == currentIndex));
        currentIndex++;
        currentTimeIndicator = audioSource.time;
        Debug.Log(currentTimeIndicator);
    }
}
