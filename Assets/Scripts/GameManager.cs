using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; set; }

    private AudioSource audioSource;
    public AudioData audioData;
    public TargetCubeSpawner spawner;

    [Range(1, 10)]
    public float targetCubeSpeed = 5f;
    [Range(0, 1)]
    public float slashAngleThreshold = .5f;
    [Range(0, 1)]
    public float slashIntensityThreshold = .5f;

    public float timeBeforeNextBeat = 0f;
    public float currentTimeIndicator = 0;
    public int currentIndex = 0;

    public Results results = new Results();

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            throw new System.Exception("Can't have multiple game manager.");
        }
    }

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    public void Play ()
    {
        results = new Results();
        spawner = FindObjectOfType<TargetCubeSpawner>();
        StartCoroutine("StartMusic");
        InvokeRepeating("StartSpawning", 0f, 60f / audioData.beatPerMinute);
        Invoke("Finish", audioData.audioClip.length + 1f);
    }

    public void Finish()
    {
        CancelInvoke("StartSpawning");

        results.Date = System.DateTime.Today.ToString();
        SceneDataManager.Instance.GoToMenuScene();
    }

    private IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(audioData.startingOffset);
        audioSource.clip = audioData.audioClip;
        audioSource.Play();
    }

    private void StartSpawning()
    {
        spawner.SpawnTargetCubes(audioData.listCubes.FindAll(t => t.Id == currentIndex));
        spawner.SpawnWall(audioData.listWalls.Find(w => w.Id == currentIndex));
        currentIndex++;
        currentTimeIndicator = audioSource.time;
    }
}
