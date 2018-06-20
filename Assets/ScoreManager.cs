using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager Instance;

    [Header("Scoring")]
    public Results results;
    public float multiplier = 1f;
    public int combo = 0;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        results = new Results();
        results.Date = System.DateTime.Today.ToString();
        results.Difficulty = GameManager.Instance.audioData.difficulty;
    }
	
	public void CorrectHit()
    {
        results.MaxHit++;
        results.CorrectHit++;
        combo++;
        if (multiplier < 10f)
        {
            multiplier += .5f;
        }

        results.Score += (int)(123f * multiplier);
    }

    public void MissHit()
    {
        results.MaxHit++;
        combo = 0;
        multiplier -= 1f;
        if (multiplier < 1f)
        {
            multiplier = 1f;
        }
    }

}
