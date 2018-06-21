using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; set; }

    [Header("Inputs")]
    private AudioSource audioSource;
    public AudioData audioData;
    public TargetCubeSpawner spawner;

    [Header("Settings")]
    [Range(1, 10)]
    public float targetCubeSpeed = 5f;
    [Range(0, 1)]
    public float slashAngleThreshold = .5f;
    [Range(0, 1)]
    public float slashIntensityThreshold = .5f;

    private float timeBeforeNextBeat = 0f;
    private int currentIndex = 0;

    [Header("Others")]
    public bool isPaused = false;
    public Results results;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(audioSource != null && audioSource.clip != null && audioSource.time >= audioData.audioClip.length - 0.1f)
        {
            Finish();
        }
    }

    // Use this for initialization
    public void Play ()
    {
        results = null;
        spawner = FindObjectOfType<TargetCubeSpawner>();
        currentIndex = 0;

        if(audioData.startingOffset < 0f)
        {
            Invoke("StartMusic", 0f);
            AtmosphereManager.Instance.StartToggleColor(audioData.beatPerMinute, 0f);
            InvokeRepeating("StartSpawning", -audioData.startingOffset, 60f / audioData.beatPerMinute);
        }
        else
        {
            Invoke("StartMusic", audioData.startingOffset);
            AtmosphereManager.Instance.StartToggleColor(audioData.beatPerMinute, audioData.startingOffset);
            InvokeRepeating("StartSpawning", 0f, 60f / audioData.beatPerMinute);
        }       
    }

    public void Finish()
    {
        CancelInvoke("StartSpawning");
        isPaused = false;

        results = new Results(ScoreManager.Instance.results);

        SceneDataManager.Instance.GoToMenuScene();
    }

    private void StartMusic()
    {
        audioSource.clip = audioData.audioClip;
        audioSource.Play();
    }
    

    private void StartSpawning()
    {
        if (!isPaused)
        {
            spawner.SpawnTargetCubes(audioData.listCubes.FindAll(t => t.Id == currentIndex));
            spawner.SpawnWall(audioData.listWalls.FindAll(w => w.Id == currentIndex));
            currentIndex++;
        }
    }

    public void Pause()
    {
        isPaused = true;
        audioSource.Pause();        
    }

    public void Unpause()
    {
        audioSource.UnPause();
        isPaused = false;
    }
}
