using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; set; }

    private AudioSource audioSource;
    private AudioLowPassFilter audioLowPassFilter;

    public void Awake()
    {
        if (Instance == null)
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
        audioLowPassFilter = GetComponent<AudioLowPassFilter>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnHeadsetCollisionDetected()
    {
        if (audioLowPassFilter != null)
        {
            audioLowPassFilter.cutoffFrequency = 1000f;
        }
    }

    public void OnHeadsetCollisionEnded()
    {
        if (audioLowPassFilter != null)
        {
            audioLowPassFilter.cutoffFrequency = 22000f;
        }
    }
}
